using System;

namespace JiraStatistic.Domain.Settings.Jira
{
    public class JiraSettings
    {
        public JiraInfo[] JiraInfos { get; set; }
    }

    public class JiraInfo
    {
        public JiraAuthSettings Auth { get; set; }
        public Uri BaseUri { get; set; }
    }
}