using System.Threading.Tasks;
using Refit;

namespace JiraStatistic.JiraClient.Clients.User
{
    public interface IJiraUserClient
    {
        [Get("/rest/api/2/myself")]
        Task<User> Myself([Property(nameof(JiraConfig))] JiraConfig jiraConfig);
    }
}