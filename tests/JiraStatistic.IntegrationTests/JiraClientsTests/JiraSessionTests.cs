using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.Domain.Settings;
using JiraStatistic.JiraClient.Clients.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraSessionTests
    {
        [Test]
        public async Task GetCurrentSessionAuthUserTest()
        {
            var client = TestStartup.ServiceProvider.GetService<IJiraSessionClient>();
            var jiraSettings = TestStartup.ServiceProvider.GetService<IOptions<JiraSettings>>()?.Value;

            var currentSessionUser = await client!.GetCurrentSessionUser();

            currentSessionUser.Should().NotBeNull();
            currentSessionUser.Name.Should().Be(jiraSettings?.Auth.Login);
        }
    }
}