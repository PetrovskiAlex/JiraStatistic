namespace JiraStatistic.JiraClient.Clients.Session
{
    public class LoginInfo
    {
        public int FailedLoginCount { get; set; }
        public int LoginCount { get; set; }
        public string LastFailedLoginTime { get; set; }
        public string PreviousLoginTime { get; set; }
    }
}