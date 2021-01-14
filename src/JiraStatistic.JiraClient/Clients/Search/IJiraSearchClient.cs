using System.Threading.Tasks;
using Refit;

namespace JiraStatistic.JiraClient.Clients.Search
{
    public interface IJiraSearchClient
    {
        [Get("/rest/api/2/search")]
        Task<SearchResponse> Search(string jql = "", string fields = "", string expand = "", int maxResults = 1_000, int startAt = 0);
    }
}