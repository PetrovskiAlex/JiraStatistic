namespace JiraStatistic.Domain.Settings.Report
{
    public class ReportSettings
    {
        public MonthReportSummarySettings MonthReportSummary { get; set; }
    }

    public class MonthReportSummarySettings
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string SavePath { get; set; }
    }
}