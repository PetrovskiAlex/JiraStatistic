using System;
using System.Net.Http;
using System.Text;
using Refit;

namespace JiraStatistic.JiraClient
{
    public interface IJiraClientFactory
    {
        T GetClient<T>(JiraConfig config);
    }

    public class JiraClientFactory : IJiraClientFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public JiraClientFactory(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        public T GetClient<T>(JiraConfig config)
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.BaseAddress = config.BaseUri;
            var userLogin = config.UserLogin;
            
            var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userLogin.Login}:{userLogin.Password}"));

            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);

            return RestService.For<T>(httpClient, new RefitSettings(new NewtonsoftJsonContentSerializer()));
        }
    }
}