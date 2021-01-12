using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.JiraClient.Clients.Session;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientTests
{
    public class JiraSession : JiraClient
    {
        [Test]
        public async Task GetCurrentSessionAuthUserTest()
        {
            using var httpClient = BuildHttpClient();

            var client = GetClient<IJiraSessionClient>(httpClient);
            var currentSessionUser = await client.GetCurrentSessionUser();

            currentSessionUser.Should().NotBeNull();
            currentSessionUser.Name.Should().Be(UserName);
        }
    }
}