using System;
using System.Text.Json;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Business.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.Reports
{
    public class MonthReportTests
    {
        private readonly IMonthSummaryReportDataProvider? _reportDataProvider;
        private readonly ReportSettings? _reportSettings;
        private readonly JiraSettings? _jiraSettings;
        private readonly IReportSaver? _excelMonthReportSaver;

        public MonthReportTests()
        {
            _excelMonthReportSaver = TestStartup.ServiceProvider.GetService<ExcelMonthReportSaver>();
            _reportDataProvider = TestStartup.ServiceProvider.GetService<IMonthSummaryReportDataProvider>();
            _reportSettings = TestStartup.ServiceProvider.GetService<IOptions<ReportSettings>>()?.Value;
            _jiraSettings = TestStartup.ServiceProvider.GetService<IOptions<JiraSettings>>()?.Value;
        }
        
        [Test]
        public async Task GetDataTest()
        {
            var reportData = await _reportDataProvider!.GetData();

            reportData.Should().NotBeNull();
        }
        
        [Test]
        public async Task ReportTest()
        {
            var monthReportTaskInfo = new Faker<ReportTaskInfo>()
                .RuleFor(p => p.Code, r => r.Random.String())
                .RuleFor(p => p.Name, r => r.Random.String())
                .RuleFor(p => p.Hours, r => r.Random.Double(100d, 200d))
                .Generate(5);
            
            var monthReportProjectInfo = new Faker<ProjectSummaryReportData>()
                .RuleFor(p => p.Name, r => r.Random.String())
                .RuleFor(p => p.ClosedHours, r => r.Random.Double(100d, 200d))
                .RuleFor(p => p.Tasks, monthReportTaskInfo.ToArray)
                .Generate();

            var monthSummaryReportData = new Faker<SummaryReportData>()
                //.RuleFor(r => r.Name, r => r.Person.FullName) //TODO
                //.RuleFor(r => r.Date, r => r.Date.Past()) //TODO
                //.RuleFor(r => r.Project, monthReportProjectInfo)
                //.RuleFor(r => r.ClosedHours, r => r.Random.Double(100d, 200d)) //TODO
                .Generate();

            //await _excelMonthReportSaver!.Save(monthSummaryReportData); //TODO

            await Task.CompletedTask;
        }
    }
}