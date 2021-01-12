using System.Threading.Tasks;
using Refit;

namespace JiraStatistic.JiraClient.Clients.Session
{
    public interface IJiraSessionClient
    {
        [Get("/rest/auth/1/session")]
        Task<CurrentUser> GetCurrentSessionUser();
    }
}