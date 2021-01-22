using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;
using JiraStatistic.JiraClient.Clients.Issue;
using JiraStatistic.JiraClient.Clients.Project;
using JiraStatistic.JiraClient.Clients.Search;
using JiraStatistic.JiraClient.Clients.Session;
using JiraStatistic.JiraClient.Clients.User;

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit{}
}

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReportDataProvider : IMonthSummaryReportDataProvider
    {
        private readonly IJiraProjectClient _projectClient;
        private readonly IJiraSearchClient _searchClient;
        private readonly IJiraUserClient _jiraUserClient;
        private readonly IJiraWorkLogClient _jiraWorkLogClient;


        public MonthSummaryReportDataProvider(
            IJiraProjectClient projectClient, 
            IJiraSearchClient searchClient,
            IJiraUserClient jiraUserClient,
            IJiraWorkLogClient jiraWorkLogClient)
        {
            _projectClient = projectClient;
            _searchClient = searchClient;
            _jiraUserClient = jiraUserClient;
            _jiraWorkLogClient = jiraWorkLogClient;
        }

        public async Task<MonthSummaryReportData> GetData(MonthReportSummarySettings monthReportSettings, JiraProjectSettings projectSettings, JiraAuthSettings? authSettings = null)
        {
            var user = await _jiraUserClient.Myself();
            var projectInfo = await _projectClient.GetProjectInfo(projectSettings.Name);

            var dateFilter = GetDateFilter(monthReportSettings);
            var issues = await GetIssues(monthReportSettings, projectSettings, dateFilter);
            var worklogs = await GetWorkLogs(issues, dateFilter, user.Name);

            return new MonthSummaryReportData
            {
                Date = GetDateFilter(monthReportSettings).Start,
                ClosedHours = Math.Round(worklogs.Sum(w => w.Hours), 1),
                Name = user.DisplayName,
                Project = new MonthReportProjectInfo
                {
                    Name = projectInfo.Name,
                    Tasks = worklogs.Select(w => new MonthReportTaskInfo
                    {
                        Code = w.Code,
                        Name = w.Summary,
                        Hours = w.Hours
                    }).ToArray(),
                    ClosedHours = Math.Round(worklogs.Sum(w => w.Hours), 1)
                }
            };
        }
        
        private async Task<List<Issue>> GetIssues(MonthReportSummarySettings monthReportSettings, JiraProjectSettings projectSettings, DateTimeFilter dateFilter)
        {
            var startAt = 0;
            var jql = $"project = {projectSettings.Name} " +
                      $"and worklogAuthor = currentUser() " +
                      $"and worklogDate >= {dateFilter.Start:yyyy-MM-dd} " +
                      $"and worklogDate <= {dateFilter.End:yyyy-MM-dd} " +
                      $" order by createdDate asc";

            var fields = "worklog, summary";
            var total = 0;

            var result = new List<Issue>();
            var searchResponse = await _searchClient.Search(jql, fields, startAt:startAt);
            result.AddRange(searchResponse.Issues);

            total += (searchResponse.Issues?.Length ?? 0);
            while (total < searchResponse.Total)
            {
                searchResponse = await _searchClient.Search(jql, fields, startAt:startAt);
                result.AddRange(searchResponse.Issues);
                total += (searchResponse.Issues?.Length ?? 0);
            }

            return result;
        }
        
        private async ValueTask<IssueTime[]> GetWorkLogs(List<Issue> issues, DateTimeFilter filter, string user)
        {
            var issuesToEnrichWorkLogs = issues.Where(i => i.Fields.Worklog.MaxResults < i.Fields.Worklog.Total).ToArray();
            foreach (var issue in issuesToEnrichWorkLogs)
            {
                var workLogs = new List<WorklogItem>();
                var issueWorkLogResponse = await _jiraWorkLogClient.GetIssueWorkLogs(issue.Id);
                workLogs.AddRange(issueWorkLogResponse.Worklogs);
                var total = issueWorkLogResponse.Worklogs.Length;
                while (total < issueWorkLogResponse.Total)
                {
                    issueWorkLogResponse = await _jiraWorkLogClient.GetIssueWorkLogs(issue.Id);
                    workLogs.AddRange(issueWorkLogResponse.Worklogs);
                    total += issueWorkLogResponse.Worklogs.Length;
                }

                issue.Fields.Worklog.Worklogs = workLogs.ToArray();
            }

            var result = issues
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

        private DateTimeFilter GetDateFilter(MonthReportSummarySettings monthReportSettings)
        {
            var dateNow = DateTime.Now;
            var year = monthReportSettings.Year <= 0 ? dateNow.Year : monthReportSettings.Year;
            var month = monthReportSettings.Month <= 0 || monthReportSettings.Month > 12
                ? dateNow.Month
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