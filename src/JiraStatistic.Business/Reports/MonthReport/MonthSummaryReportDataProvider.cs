using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;
using JiraStatistic.JiraClient.Clients.Project;
using JiraStatistic.JiraClient.Clients.Search;
using JiraStatistic.JiraClient.Clients.Session;

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
        private readonly IJiraSessionClient _sessionClient;

        public MonthSummaryReportDataProvider(
            IJiraProjectClient projectClient, 
            IJiraSearchClient searchClient,
            IJiraSessionClient sessionClient)
        {
            _projectClient = projectClient;
            _searchClient = searchClient;
            _sessionClient = sessionClient;
        }

        public async Task<MonthSummaryReportData> GetData(MonthReportSummarySettings monthReportSettings, JiraProjectSettings projectSettings, JiraAuthSettings? authSettings = null)
        {
            //var projectInfo = await _projectClient.GetProjectInfo(projectSettings.Name);

            var dateNow = DateTime.Now;

            var issues = await GetIssues(monthReportSettings, projectSettings, dateNow);
            var worklogs = GetWorklogs(issues);

            return new MonthSummaryReportData
            {
                Date = GetDateFilter(monthReportSettings).Start,
                ClosedHours = Math.Round(worklogs.Sum(w => w.Hours), 1),
                Project = new MonthReportProjectInfo
                {
                    //Name = projectInfo.Name,
                    Tasks = worklogs.Select(w => new MonthReportTaskInfo
                    {
                        Name = w.Summary,
                        Hours = w.Hours
                    }).ToArray(),
                    ClosedHours = Math.Round(worklogs.Sum(w => w.Hours), 1)
                }
            };
        }
        
        private async Task<List<Issue>> GetIssues(MonthReportSummarySettings monthReportSettings, JiraProjectSettings projectSettings, DateTime dateNow)
        {
            var dateFilter = GetDateFilter(monthReportSettings);

            var totalResult = 0;
            var startAt = 0;
            var jql = $"project = {projectSettings.Name} " +
                      $"and worklogAuthor = currentUser() " +
                      $"and worklogDate >= {dateFilter.Start:yyyy-mm-dd} " +
                      $"and worklogDate <= {dateFilter.End:yyyy-mm-dd} " +
                      $"and order by createdDate asc";

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

        private IssueTime[] GetWorklogs(List<Issue> issues)
        {
            return issues
                .Select(issue => new
                {
                    issue.Fields.Summary,
                    Hours = Math.Round(
                        (issue.Fields.Worklog?.Worklogs?.Sum(w => w.TimeSpentSeconds) / 60 / 60.0) ?? 0, 1)
                })
                .GroupBy(w => w.Summary)
                .Select(group => new IssueTime(@group.Key, @group.Sum(g => g.Hours)))
                .ToArray();
        }

        private record DateTimeFilter(DateTime Start, DateTime End);
        
        private record IssueTime(string Summary, double Hours);
    }
}