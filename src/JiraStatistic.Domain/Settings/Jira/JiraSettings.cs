using System;
using JiraStatistic.Domain.Settings.Jira;

namespace JiraStatistic.Domain.Settings
{
    public class JiraSettings
    {
        public JiraAuthSettings Auth { get; set; }
        public Uri BaseUri { get; set; }
        public JiraProjectSettings ProjectInfo { get; set; }
    }
}