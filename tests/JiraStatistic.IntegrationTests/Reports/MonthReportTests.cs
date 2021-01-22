using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Business.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.Reports
{
    public class MonthReportTests
    {
        private readonly IMonthSummaryReportDataProvider? _reportDataProvider;
        private readonly ReportSettings? _reportSettings;
        private readonly JiraSettings? _jiraSettings;

        public MonthReportTests()
        {
            _reportDataProvider = TestStartup.ServiceProvider.GetService<IMonthSummaryReportDataProvider>();
            _reportSettings = TestStartup.ServiceProvider.GetService<IOptions<ReportSettings>>()?.Value;
            _jiraSettings = TestStartup.ServiceProvider.GetService<IOptions<JiraSettings>>()?.Value;
        }
        
        [Test]
        public async Task GetDataTest()
        {
            var reportData = await _reportDataProvider!.GetData(_reportSettings!.MonthReportSummary!, _jiraSettings!.ProjectInfo!);

            reportData.Should().NotBeNull();
        }
        
        [Test]
        public async Task ReportTest()
        {
            var reportData = await _reportDataProvider!.GetData(_reportSettings!.MonthReportSummary!, _jiraSettings!.ProjectInfo!);

            var wb = MonthSummaryReport.GenerateExcel(reportData);
            wb.SaveAs($"Закрытые часы {_reportSettings!.MonthReportSummary!.Year}_{_reportSettings!.MonthReportSummary!.Month}.xlsx");
        }
    }
}