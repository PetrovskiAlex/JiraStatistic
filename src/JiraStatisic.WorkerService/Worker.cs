using System;
using System.Threading;
using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JiraStatistic.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<Worker> _logger;

        public Worker(IServiceProvider serviceProvider, ILogger<Worker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            
            var monthSummaryReport = _serviceProvider.GetService<IMonthSummaryReport>() ?? throw new NullReferenceException();
            await monthSummaryReport.MakeReport();

            _logger.LogInformation("Worker ended at: {Time}", DateTimeOffset.Now);
        }
    }
}