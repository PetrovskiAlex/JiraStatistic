using System;
using JiraStatistic.JiraClient;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace JiraStatistic.WorkerService
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureRefitClient<T>(this ServiceCollection serviceCollection) where T : class
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddRefitClient<T>().ConfigurePrimaryHttpMessageHandler<RequestHttpHandler>();
        }
    }
}