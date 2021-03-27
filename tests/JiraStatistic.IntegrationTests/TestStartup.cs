using System;
using System.IO;
using JiraStatistic.Business.Abstractions.Reports;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Business.Reports;
using JiraStatistic.Business.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;
using JiraStatistic.JiraClient;
using JiraStatistic.JiraClient.Clients.Issue;
using JiraStatistic.JiraClient.Clients.Project;
using JiraStatistic.JiraClient.Clients.Search;
using JiraStatistic.JiraClient.Clients.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Refit;

namespace JiraStatistic.IntegrationTests
{
    [SetUpFixture]
    internal class TestStartup
    {
        private static IConfiguration Configuration { get; set; }
        public static IServiceProvider ServiceProvider { get; private set; }
        
        [OneTimeSetUp]
        public static void Setup()
        {
            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..");
            Configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", true)
                .Build();

            var serviceCollection = new ServiceCollection();

            var jiraInfos = Configuration.GetSection(nameof(JiraSettings)).Get<JiraInfo[]>();
            serviceCollection.Configure<JiraSettings>(options => options.JiraInfos = jiraInfos);
            serviceCollection.Configure<ReportSettings>(Configuration.GetSection(nameof(ReportSettings)));

            serviceCollection.AddTransient<IJiraClientFactory, JiraClientFactory>();
            serviceCollection.AddRefitClient<IJiraProjectClient>();
            serviceCollection.AddRefitClient<IJiraSearchClient>();
            serviceCollection.AddRefitClient<IJiraUserClient>();
            serviceCollection.AddRefitClient<IJiraWorkLogClient>();
            
            serviceCollection.AddScoped<IMonthSummaryReportDataProvider, MonthSummaryReportDataProvider>();
            serviceCollection.AddScoped<ExcelMonthReportSaver>();
            serviceCollection.AddSingleton<IReportFactory, ReportFactory>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}