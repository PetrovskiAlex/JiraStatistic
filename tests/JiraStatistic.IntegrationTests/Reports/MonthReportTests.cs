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
            var monthReportSummarySettings = TestStartup.ServiceProvider.GetService<IOptions<MonthReportSummarySettings>>()?.Value;
            var jiraProjectSettings = TestStartup.ServiceProvider.GetService<IOptions<JiraProjectSettings>>()?.Value;

            var reportData = await dataProvider!.GetData(monthReportSummarySettings!, jiraProjectSettings!);

            reportData.Should().NotBeNull();
        }
    }
}