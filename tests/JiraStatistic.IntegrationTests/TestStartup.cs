using System;
using System.IO;
using JiraStatistic.Business.Abstractions.Reports;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Business.Reports;
using JiraStatistic.Business.Reports.MonthReport;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Report;
using JiraStatistic.JiraClient.Clients.Issue;
using JiraStatistic.JiraClient.Clients.Project;
using JiraStatistic.JiraClient.Clients.Search;
using JiraStatistic.JiraClient.Clients.Session;
using JiraStatistic.JiraClient.Clients.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

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
            serviceCollection.Configure<JiraSettings>(Configuration.GetSection(nameof(JiraSettings)));
            serviceCollection.Configure<ReportSettings>(Configuration.GetSection(nameof(ReportSettings)));

            serviceCollection.ConfigureRefitClient<IJiraSessionClient>();
            serviceCollection.ConfigureRefitClient<IJiraProjectClient>();
            serviceCollection.ConfigureRefitClient<IJiraSearchClient>();
            serviceCollection.ConfigureRefitClient<IJiraUserClient>();
            serviceCollection.ConfigureRefitClient<IJiraWorkLogClient>();
            
            serviceCollection.AddScoped<IMonthSummaryReportDataProvider, MonthSummaryReportDataProvider>();
            serviceCollection.AddScoped<ExcelMonthReportSaver>();
            serviceCollection.AddSingleton<IReportFactory, ReportFactory>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}