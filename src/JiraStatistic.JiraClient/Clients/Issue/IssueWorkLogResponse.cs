using JiraStatistic.JiraClient.Clients.Search;

namespace JiraStatistic.JiraClient.Clients.Issue
{
    public class IssueWorkLogResponse : BaseStatisticResponse
    {
        public WorklogItem[] Worklogs { get; set; }
    }
}