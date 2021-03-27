using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.JiraClient;
using JiraStatistic.JiraClient.Clients.Session;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraSessionTests : JiraClientTests
    {
        [Test]
        public async Task GetCurrentSessionAuthUserTest()
        {
            var jiraClientFactory = TestStartup.ServiceProvider.GetService<IJiraClientFactory>();
            var client = jiraClientFactory!.GetClient<IJiraSessionClient>(JiraConfig);

            var currentSessionUser = await client!.GetUser();

            currentSessionUser.Should().NotBeNull();
            currentSessionUser.Name.Should().Be(JiraConfig.UserLogin.Login);
        }
    }
}