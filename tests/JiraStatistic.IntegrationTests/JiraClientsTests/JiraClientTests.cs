using System.Linq;
using System.Text.Json;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.JiraClient;
using JiraStatistic.JiraClient.Clients.User;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraClientTests
    {
        protected JiraConfig JiraConfig;
        
        public JiraClientTests()
        {
            var settings = TestStartup.ServiceProvider.GetService<IOptions<JiraSettings>>()!.Value;
            var jiraInfo = settings.JiraInfos.First();
            JiraConfig = new JiraConfig(new UserLogin(jiraInfo.Auth.Login, jiraInfo.Auth.Password), jiraInfo.BaseUri);
        }
    }
}