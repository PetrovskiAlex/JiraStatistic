using System;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public class SummaryReportData
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double ClosedHours { get; set; }
        public ReportProjectInfo Project { get; set; }
    }

    public class ReportProjectInfo
    {
        public string Name { get; set; }
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