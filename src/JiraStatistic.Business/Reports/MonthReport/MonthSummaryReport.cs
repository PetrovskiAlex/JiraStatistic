﻿using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Report;

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReport : IMonthSummaryReport
    {
        private readonly IMonthSummaryReportDataProvider _reportDataProvider;

        public MonthSummaryReport(IMonthSummaryReportDataProvider reportDataProvider)
        {
            _reportDataProvider = reportDataProvider;
        }

        public async Task MakeReport(MonthReportSummarySettings monthReportSummarySettings, JiraProjectSettings jiraProjectSettings)
        {
            var reportData = await _reportDataProvider.GetData(monthReportSummarySettings, jiraProjectSettings);
            //Make a report
            //Save report

            throw new System.NotImplementedException();
        }
    }
}