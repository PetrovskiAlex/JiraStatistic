using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;
using JiraStatistic.JiraClient;
using JiraStatistic.JiraClient.Clients.Issue;
using JiraStatistic.JiraClient.Clients.Project;
using JiraStatistic.JiraClient.Clients.Search;
using JiraStatistic.JiraClient.Clients.User;
using Microsoft.Extensions.Options;

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit{}
}

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReportDataProvider : IMonthSummaryReportDataProvider
    {
        private readonly IJiraClientFactory _jiraClientFactory;
        private readonly JiraSettings _jiraSettings;
        private readonly ReportSettings _reportSettings;

        public MonthSummaryReportDataProvider(
            IJiraClientFactory jiraClientFactory,
            IOptions<JiraSettings> jiraSettings,
            IOptions<ReportSettings> reportSettings)
        {
            _jiraClientFactory = jiraClientFactory;
            _jiraSettings = jiraSettings.Value;
            _reportSettings = reportSettings.Value;
        }

        public async Task<SummaryReportData> GetData()
        {
            var dateFilter = GetDateFilter();

            var reportData = new SummaryReportData
            {
                Date = dateFilter.Start,
            };

            var projectReports = new List<ProjectSummaryReportData>();
            foreach (var jiraInfo in _jiraSettings.JiraInfos)
            {
                await foreach (var projectsInfo in GetProjectsInfoFromJira(jiraInfo, dateFilter))
                {
                    projectReports.Add(projectsInfo);
                }    
            }

            reportData.Projects = projectReports.ToArray();
            reportData.ClosedHours = projectReports.Sum(p => p.ClosedHours);
            return reportData;
        }

        private async IAsyncEnumerable<ProjectSummaryReportData> GetProjectsInfoFromJira(JiraInfo jiraInfo, DateTimeFilter dateFilter)
        {
            var jiraConfig = new JiraConfig(new UserLogin(jiraInfo.Auth.Login, jiraInfo.Auth.Password), jiraInfo.BaseUri);
            var jiraUserClient = _jiraClientFactory.GetClient<IJiraUserClient>(jiraConfig);
            var user = await jiraUserClient.Myself(jiraConfig);
            var issues = await GetIssues(jiraConfig, dateFilter);
            var projects = GetProjectsFromIssues(issues);

            foreach (var project in projects)
            {
                var workLogs = await GetWorkLogs(jiraConfig, issues, dateFilter, project.Key, user.Name);
                var projectData = new ProjectSummaryReportData
                {
                    Name = project.Name,
                    Tasks = workLogs.Select(w => new ReportTaskInfo
                    {
                        Code = w.Code,
                        Name = w.Summary,
                        Hours = w.Hours
                    }).ToArray(),
                    ClosedHours = Math.Round(workLogs.Sum(w => w.Hours), 1)
                };

                yield return projectData;
            }
        }

        private ProjectInfo[] GetProjectsFromIssues(List<Issue> issues)
        {
            return issues
                .Where(i => i.Fields?.Project != null)
                .GroupBy(i => new {i.Fields.Project.Key, i.Fields.Project.Name})
                .Select(g => new ProjectInfo
                {
                    Key = g.Key.Key,
                    Name = g.Key.Name
                })
                .ToArray();
        }
        
        private async Task<List<Issue>> GetIssues(JiraConfig jiraConfig, DateTimeFilter dateFilter)
        {
            var startAt = 0;
            var jql = "worklogAuthor = currentUser() " +
                      $"and worklogDate >= {dateFilter.Start:yyyy-MM-dd} " +
                      $"and worklogDate <= {dateFilter.End:yyyy-MM-dd} " +
                      $" order by createdDate asc";

            var fields = "worklog, summary, project";

            var searchClient = _jiraClientFactory.GetClient<IJiraSearchClient>(jiraConfig);
            var result = await JiraClientHelper.GetAll(() => searchClient.Search(jql, fields, startAt: startAt), response => response.Issues);
            return result;
        }
        
        private async ValueTask<IssueTime[]> GetWorkLogs(JiraConfig jiraConfig, List<Issue> issues, DateTimeFilter filter, string projectKey, string user)
        {
            var issuesToEnrichWorkLogs = issues
                .Where(i => i.Fields.Worklog.MaxResults < i.Fields.Worklog.Total && i.Fields.Project.Key == projectKey).ToArray();

            var jiraWorkLogClient = _jiraClientFactory.GetClient<IJiraWorkLogClient>(jiraConfig);
            foreach (var issue in issuesToEnrichWorkLogs)
            {
                var workLogs = await JiraClientHelper.GetAll(() => jiraWorkLogClient.GetIssueWorkLogs(issue.Id), response => response.Worklogs);
                issue.Fields.Worklog.Worklogs = workLogs.ToArray();
            }

            var result = issues
                .Where(i => i.Fields.Project.Key == projectKey)
                .Select(issue => new
                {
                    issue.Key,
                    issue.Fields.Summary,
                    Hours = Math.Round(
                        (issue.Fields.Worklog?.Worklogs?
                            .Where(w => w.Author.Name == user)
                            .Where(w => w.Started >= filter.Start && w.Started <= filter.End)
                            .Sum(w => w.TimeSpentSeconds) / 60 / 60.0) ?? 0, 1)
                })
                .GroupBy(w => w.Key)
                .Select(group =>
                    new IssueTime(@group.Key, @group.Select(g => g.Summary).FirstOrDefault() ?? string.Empty, @group.Sum(g => g.Hours)))
                .ToArray();
            
            return result;
        }

        private DateTimeFilter GetDateFilter()
        {
            var monthReportSettings = _reportSettings.ReportSummary;
            
            var dateNow = DateTime.Now;
            var year = monthReportSettings.Year <= 0 ? dateNow.Year : monthReportSettings.Year;
            var month = monthReportSettings.Month <= 0 || monthReportSettings.Month > 12
                ? dateNow.Month > 1 ? dateNow.Month - 1 : dateNow.Month
                : monthReportSettings.Month;
            var daysInMonth = DateTime.DaysInMonth(year, month);
            var start = new DateTime(year, month, 1);
            var end = new DateTime(year, month, daysInMonth); 
            return new DateTimeFilter(start, end);
        }

        private record DateTimeFilter(DateTime Start, DateTime End);

        private record IssueTime(string Code, string Summary, double Hours);
    }
}