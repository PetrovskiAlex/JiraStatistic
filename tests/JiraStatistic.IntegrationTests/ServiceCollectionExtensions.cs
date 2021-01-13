using System;
using System.Text;
using JiraStatistic.Domain.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;

namespace JiraStatistic.IntegrationTests
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureRefitClient<T>(this ServiceCollection serviceCollection) where T : class
        {
            if (serviceCollection == null) throw new ArgumentNullException(nameof(serviceCollection));

            serviceCollection.AddRefitClient<T>()
                .ConfigureHttpClient((provider, httpClient) =>
                {
                    var jiraSettings = provider.GetService<IOptions<JiraSettings>>()?.Value;

                    httpClient.BaseAddress = jiraSettings!.BaseUri;
                    
                    var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{jiraSettings!.Auth.Login}:{jiraSettings!.Auth.Password}"));
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                });
        }
    }
}