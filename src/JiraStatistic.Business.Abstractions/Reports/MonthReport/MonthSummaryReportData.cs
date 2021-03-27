using System;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public class SummaryReportData
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ProjectSummaryReportData[] Projects { get; set; }
        public double ClosedHours { get; set; }
    }

    public class ProjectSummaryReportData
    {
        public string Name { get; set; } = null!;
        public double ClosedHours { get; set; }
        public ReportTaskInfo[] Tasks { get; set; }
    }

    public class ReportTaskInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Hours { get; set; }
    }
}