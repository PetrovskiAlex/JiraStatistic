using System;

namespace JiraStatistic.JiraClient.Clients.Search
{
    public class Issue : BaseResponse
    {
        public IssueFields Fields { get; set; }
    }

    public class IssueFields
    {
        public string Summary { get; set; }
        public WorkLog Worklog { get; set; }
    }

    public class WorkLog : BaseStatisticResponse
    {
        public WorklogItem[] Worklogs { get; set; }
    }

    public class WorklogItem
    {
        public string Id { get; set; }
        public string Self { get; set; }
        public User Author { get; set; }
        public User UpdateAuthor { get; set; }
        public string Comment { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Started { get; set; }
        public string TimeSpent { get; set; }
        public int TimeSpentSeconds { get; set; }
    }

    public class User
    {
        public string Self { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string EmailAddress { get; set; }
        public string DisplayName { get; set; }
    }
}