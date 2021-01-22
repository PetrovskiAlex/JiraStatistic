using System;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public class MonthSummaryReportData
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double ClosedHours { get; set; }
        public MonthReportProjectInfo Project { get; set; }
    }

    public class MonthReportProjectInfo
    {
        public string Name { get; set; }
        public double ClosedHours { get; set; }
        public MonthReportTaskInfo[] Tasks { get; set; }
    }

    public class MonthReportTaskInfo
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Hours { get; set; }
    }
}