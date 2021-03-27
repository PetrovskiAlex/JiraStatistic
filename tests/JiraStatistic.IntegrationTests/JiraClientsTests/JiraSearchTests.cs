using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using JiraStatistic.JiraClient;
using JiraStatistic.JiraClient.Clients.Search;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraSearchTests : JiraClientTests
    {
        [Test]
        public async Task SearchTest()
        {
            var jiraClientFactory = TestStartup.ServiceProvider.GetService<IJiraClientFactory>();
            var client = jiraClientFactory!.GetClient<IJiraSearchClient>(JiraConfig);

            var jql = "project = MC and worklogAuthor = currentUser() and worklogDate > startOfWeek()";
            var fields = "worklog, summary";
            var response = await client!.Search(jql, fields);
        }

        [Test]
        public async Task SearchProjectsWhereIWasParticipatedTest()
        {
            var jiraClientFactory = TestStartup.ServiceProvider.GetService<IJiraClientFactory>();
            var client = jiraClientFactory!.GetClient<IJiraSearchClient>(JiraConfig);

            var jql = "worklogAuthor = currentUser() and worklogDate >= '2021-02-01'";
            var fields = "project";
            var response = await client!.Search(jql, fields);
            var projects = response.Issues?
                .GroupBy(i => i.Fields?.Project?.Key)
                .Select(g => g.Key)
                .ToArray();

            projects.Should().Contain(new[] { "PROD", "HR", "STAF" });
        }
    }
}