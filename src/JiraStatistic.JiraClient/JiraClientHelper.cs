using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JiraStatistic.JiraClient.Clients;

namespace JiraStatistic.JiraClient
{
    public static class JiraClientHelper
    {
        public static async Task<List<TResponse>> GetAll<T, TResponse>(Func<Task<T>> getData, Func<T, IEnumerable<TResponse>> selector) where T : BaseStatisticResponse
        {
            var result = new List<TResponse>();

            var issueWorkLogResponse = await getData();
            var data = selector(issueWorkLogResponse);

            result.AddRange(data);
            var total = data.Count();
            while (total < issueWorkLogResponse.Total)
            {
                issueWorkLogResponse = await getData();
                result.AddRange(selector(issueWorkLogResponse));
                total += data.Count();
            }

            return result;
        }
    }
}