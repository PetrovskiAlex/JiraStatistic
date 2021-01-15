using System;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public class MonthSummaryReportData
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public float ClosedHours { get; set; }
        public MonthReportProjectInfo[] Projects { get; set; }
    }

    public class MonthReportProjectInfo
    {
        public string Name { get; set; }
        public float ClosedHours { get; set; }
        public MonthReportTaskInfo[] Tasks { get; set; }
    }

    public class MonthReportTaskInfo
    {
        public string Name { get; set; }
        public float Hours { get; set; }
    }
}