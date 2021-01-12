using System;

namespace JiraStatistic.JiraClient.Clients.Session
{
    public class CurrentUser
    {
        public Uri Self { get; set; }
        public string Name { get; set; }
        public LoginInfo LoginInfo { get; set; }
    }
}