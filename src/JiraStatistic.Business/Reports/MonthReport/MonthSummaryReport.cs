using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.Options;

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReport : IMonthSummaryReport
    {
        private readonly IMonthSummaryReportDataProvider _reportDataProvider;
        private readonly IReportFactory _reportFactory;
        private readonly ReportSettings _reportSettings;
        private readonly JiraSettings _jiraSettings;

        public MonthSummaryReport(IMonthSummaryReportDataProvider reportDataProvider, IReportFactory reportFactory, IOptions<ReportSettings> reportSettings, IOptions<JiraSettings> jiraSettings)
        {
            _reportDataProvider = reportDataProvider;
            _reportFactory = reportFactory;
            _reportSettings = reportSettings.Value;
            _jiraSettings = jiraSettings.Value;
        }

        public async Task MakeReport()
        {
            var reportData = await _reportDataProvider.GetData();
            
            var reportSaver = _reportFactory.CreateReportSaver(_reportSettings.ReportSummary.DocumentType);
            //await Task.Run(() => reportSaver.Save(reportData)); //TODO
        }
    }
}