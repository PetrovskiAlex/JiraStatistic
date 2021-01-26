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
    public class Startup
    {
        private static IConfiguration Configuration { get; set; }
        
        public static void ConfigureDelegate(HostBuilderContext arg1, IServiceCollection services)
        {
            ConfigureOptions(services);

            services.ConfigureRefitClient<IJiraSessionClient>();
            services.ConfigureRefitClient<IJiraProjectClient>();
            services.ConfigureRefitClient<IJiraSearchClient>();
            services.ConfigureRefitClient<IJiraUserClient>();
            services.ConfigureRefitClient<IJiraWorkLogClient>();
            services.AddScoped<IMonthSummaryReportDataProvider, MonthSummaryReportDataProvider>();
            services.AddScoped<ExcelMonthReportSaver>();
            services.AddSingleton<IReportFactory, ReportFactory>();
            services.AddHostedService<Worker>();
        }

        private static void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<JiraSettings>(Configuration.GetSection(nameof(JiraSettings)));
            services.Configure<ReportSettings>(Configuration.GetSection(nameof(ReportSettings)));
        }
    }
}