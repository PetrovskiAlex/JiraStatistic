using System;

namespace JiraStatistic.Domain.Settings
{
    public class JiraSettings
    {
        public JiraAuthSettings Auth { get; set; }
        public Uri BaseUri { get; set; }
        public JiraProjectInfo ProjectInfo { get; set; }
    }
}