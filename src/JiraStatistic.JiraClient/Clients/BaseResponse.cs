namespace JiraStatistic.JiraClient.Clients
{
    public class BaseResponse
    {
        public string Expand { get; set; }
        public string Id { get; set; }
        public string Self { get; set; }
        public string Key { get; set; }
    }

    public class BaseStatisticResponse
    {
        public int StartAt { get; set; }
        public int Total { get; set; }
        public int MaxResults { get; set; }
    }
}