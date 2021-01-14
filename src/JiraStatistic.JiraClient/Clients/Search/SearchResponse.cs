namespace JiraStatistic.JiraClient.Clients.Search
{
    public class SearchResponse : BaseStatisticResponse
    {
        public string Expand { get; set; }
        public Issue[] Issues { get; set; }
    }
}