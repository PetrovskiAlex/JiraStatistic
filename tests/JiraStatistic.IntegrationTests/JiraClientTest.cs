using System;
using System.Net.Http;
using System.Text;
using Refit;

namespace JiraStatistic.IntegrationTests
{
    public class JiraClientTest
    {
        public Uri JiraBaseUri { get; set; } = new Uri("");
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";

        protected HttpClient BuildHttpClient()
        {
            return new HttpClient
            {
                BaseAddress = JiraBaseUri
            };
        }

        public T GetClient<T>(HttpClient httpClient)
        {
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserName}:{Password}"));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);

            return RestService.For<T>(httpClient);
        }
    }
}