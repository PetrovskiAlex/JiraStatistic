using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.Domain.Settings;
using JiraStatistic.JiraClient.Clients.Project;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraProjectTests
    {
        [Test]
        public async Task GetProjectInfo()
        {
            var client = TestStartup.ServiceProvider.GetService<IJiraProjectClient>();
            var jiraSettings = TestStartup.ServiceProvider.GetService<IOptions<JiraSettings>>()?.Value;

            var projectInfo = await client!.GetProjectInfo(jiraSettings!.ProjectInfo.Name);

            projectInfo.Should().NotBeNull();
            projectInfo.Key.Should().Be(jiraSettings.ProjectInfo.Name);
            projectInfo.Id.Should().NotBeEmpty();
        }
    }
}