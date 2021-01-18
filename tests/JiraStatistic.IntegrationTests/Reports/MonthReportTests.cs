using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.Reports
{
    public class MonthReportTests
    {
        [Test]
        public async Task GetDataTest()
        {
            var dataProvider = TestStartup.ServiceProvider.GetService<IMonthSummaryReportDataProvider>();
            var monthReportSummarySettings = TestStartup.ServiceProvider.GetService<IOptions<ReportSettings>>()?.Value;
            var jiraProjectSettings = TestStartup.ServiceProvider.GetService<IOptions<JiraSettings>>()?.Value;

            var reportData = await dataProvider!.GetData(monthReportSummarySettings!.MonthReportSummary!, jiraProjectSettings!.ProjectInfo!);

            reportData.Should().NotBeNull();
        }
    }
}