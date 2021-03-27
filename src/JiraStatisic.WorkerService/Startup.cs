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
using JiraStatistic.JiraClient.Clients.Session;
using JiraStatistic.JiraClient.Clients.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Refit;

namespace JiraStatistic.WorkerService
{
    public static class Startup
    {
        public static void ConfigureDelegate(HostBuilderContext arg1, IServiceCollection services)
        {
            ConfigureOptions(arg1.Configuration, services);

            services.AddTransient<IJiraClientFactory, JiraClientFactory>();
            services.AddRefitClient<IJiraSessionClient>();
            services.AddRefitClient<IJiraProjectClient>();
            services.AddRefitClient<IJiraSearchClient>();
            services.AddRefitClient<IJiraUserClient>();
            services.AddRefitClient<IJiraWorkLogClient>();
            services.AddTransient<IMonthSummaryReportDataProvider, MonthSummaryReportDataProvider>();
            services.AddTransient<ExcelMonthReportSaver>();
            services.AddTransient<IMonthSummaryReport, MonthSummaryReport>();
            services.AddTransient<IReportFactory, ReportFactory>();
            services.AddHostedService<Worker>();
        }

        private static void ConfigureOptions(IConfiguration configuration, IServiceCollection services)
        {
            var jiraInfos = configuration.GetSection(nameof(JiraSettings)).Get<JiraInfo[]>();
            services.Configure<JiraSettings>(options => options.JiraInfos = jiraInfos);
            services.Configure<ReportSettings>(configuration.GetSection(nameof(ReportSettings)));
        }
    }
}