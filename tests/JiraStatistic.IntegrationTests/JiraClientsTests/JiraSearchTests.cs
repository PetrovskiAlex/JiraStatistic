using System;
using System.Threading.Tasks;
using JiraStatistic.JiraClient.Clients.Search;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace JiraStatistic.IntegrationTests.JiraClientsTests
{
    public class JiraSearchTests
    {
        [Test]
        public async Task SearchTest()
        {
            var client = TestStartup.ServiceProvider.GetService<IJiraSearchClient>();

            var jql = "project = SURFNS and worklogAuthor = currentUser() and worklogDate > startOfWeek()";
            var fields = "worklog, summary";
            var response = await client!.Search(jql, fields);
        }
    }
}