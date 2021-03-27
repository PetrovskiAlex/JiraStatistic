using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JiraStatistic.JiraClient.Clients.User;

namespace JiraStatistic.JiraClient
{
    public class RequestHttpHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if(request.Properties.ContainsKey(nameof(JiraConfig)))
            {
                var jiraConfig = (JiraConfig) request.Properties[nameof(JiraConfig)];
                var userLogin = jiraConfig.UserLogin;

                var uriBuilder = new UriBuilder(jiraConfig.BaseUri)
                {
                    Path = request.RequestUri.LocalPath,
                    Query = request.RequestUri.Query,
                };
                request.RequestUri = uriBuilder.Uri;

                var authString = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userLogin.Login}:{userLogin.Password}"));

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", authString);
            }

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
    }
}