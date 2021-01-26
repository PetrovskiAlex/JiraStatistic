namespace JiraStatistic.Domain.Settings.Report
{
    public class ReportSettings
    {
        public ReportSummarySettings ReportSummary { get; set; }
    }

    public class ReportSummarySettings
    {
        public DocumentTypeEnum DocumentType { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string SavePath { get; set; }
    }
}