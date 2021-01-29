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
using Microsoft.Extensions.Hosting;

namespace JiraStatisic.WorkerService
{
    public static class Startup
    {
        public static void ConfigureDelegate(HostBuilderContext arg1, IServiceCollection services)
        {
            ConfigureOptions(arg1.Configuration, services);

            services.ConfigureRefitClient<IJiraSessionClient>();
            services.ConfigureRefitClient<IJiraProjectClient>();
            services.ConfigureRefitClient<IJiraSearchClient>();
            services.ConfigureRefitClient<IJiraUserClient>();
            services.ConfigureRefitClient<IJiraWorkLogClient>();
            services.AddTransient<IMonthSummaryReportDataProvider, MonthSummaryReportDataProvider>();
            services.AddTransient<ExcelMonthReportSaver>();
            services.AddTransient<IMonthSummaryReport, MonthSummaryReport>();
            services.AddTransient<IReportFactory, ReportFactory>();
            services.AddHostedService<Worker>();
        }

        private static void ConfigureOptions(IConfiguration configuration, IServiceCollection services)
        {
            services.Configure<JiraSettings>(configuration.GetSection(nameof(JiraSettings)));
            services.Configure<ReportSettings>(configuration.GetSection(nameof(ReportSettings)));
        }
    }
}