using System.Threading.Tasks;
using Refit;

namespace JiraStatistic.JiraClient.Clients.Issue
{
    public interface IJiraWorkLogClient
    {
        [Get("/rest/api/2/issue/{issueIdOrKey}/worklog")]
        Task<IssueWorkLogResponse> GetIssueWorkLogs(string issueIdOrKey);
    }
}