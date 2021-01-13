using System.Threading.Tasks;
using Refit;

namespace JiraStatistic.JiraClient.Clients.Project
{
    public interface IJiraProjectClient
    {
        [Get("/rest/api/2/project/{projectIdOrKey}")]
        Task<ProjectInfo> GetProjectInfo(string projectIdOrKey);
    }
}