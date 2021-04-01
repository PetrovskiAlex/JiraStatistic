using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.Options;

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReport : IMonthSummaryReport
    {
        private readonly IMonthSummaryReportDataProvider _reportDataProvider;
        private readonly IReportFactory _reportFactory;
        private readonly ReportSettings _reportSettings;

        public MonthSummaryReport(IMonthSummaryReportDataProvider reportDataProvider, IReportFactory reportFactory,
            IOptions<ReportSettings> reportSettings)
        {
            _reportDataProvider = reportDataProvider;
            _reportFactory = reportFactory;
            _reportSettings = reportSettings.Value;
        }

        public async Task MakeReport()
        {
            var reportData = await _reportDataProvider.GetData();
            
            var reportSaver = _reportFactory.CreateReportSaver(_reportSettings.ReportSummary.DocumentType);
            await Task.Run(() => reportSaver.Save(reportData));
        }
    }
}