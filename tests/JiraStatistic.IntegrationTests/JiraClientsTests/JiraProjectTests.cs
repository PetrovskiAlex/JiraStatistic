using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.JiraClient;
using JiraStatistic.JiraClient.Clients.Project;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraProjectTests : JiraClientTests
    {
        [Test]
        public async Task GetProjectInfo()
        {
            var jiraClientFactory = TestStartup.ServiceProvider.GetService<IJiraClientFactory>();
            var client = jiraClientFactory!.GetClient<IJiraProjectClient>(JiraConfig);

            var projectInfo = await client!.GetProjectInfo("PROD");

            projectInfo.Should().NotBeNull();
            projectInfo.Key.Should().Be("PROD");
            projectInfo.Id.Should().NotBeEmpty();
        }
    }
}