using System;
using System.IO;
using System.Text;
using JiraStatistic.Domain.Settings;
using JiraStatistic.JiraClient.Clients.Session;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
            serviceCollection.Configure<JiraSettings>(Configuration.GetSection(nameof(JiraSettings)));

            serviceCollection.AddRefitClient<IJiraSessionClient>()
                .ConfigureHttpClient((provider, httpClient) =>
                {
                    var jiraSettings = provider.GetService<IOptions<JiraSettings>>()?.Value;

                    httpClient.BaseAddress = jiraSettings!.BaseUri;
                    
                    var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{jiraSettings!.Auth.Login}:{jiraSettings!.Auth.Password}"));
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
                });
            
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}