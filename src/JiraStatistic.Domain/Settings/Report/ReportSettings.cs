namespace JiraStatistic.Domain.Settings.Report
{
    public class ReportSettings
    {
        public MonthSummarySettings MonthSummary { get; set; }
    }

    public class MonthSummarySettings
    {
        public string Month { get; set; }
        public string SavePath { get; set; }
    }
}