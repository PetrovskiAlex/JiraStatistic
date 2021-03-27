using System;
using JiraStatistic.JiraClient.Clients.User;

namespace JiraStatistic.JiraClient
{
    public class JiraConfig
    {
        public JiraConfig(UserLogin userLogin, Uri baseUri)
        {
            UserLogin = userLogin ?? throw new ArgumentNullException(nameof(userLogin));
            BaseUri = baseUri ?? throw new ArgumentNullException(nameof(baseUri));
        }

        public UserLogin UserLogin { get; }
        public Uri BaseUri { get; }
    }
}