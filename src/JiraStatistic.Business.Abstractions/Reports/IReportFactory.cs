using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Report;

namespace JiraStatistic.Business.Abstractions.Reports
{
    public interface IReportFactory
    {
        IReportSaver CreateReportSaver(DocumentTypeEnum documentType);
    }
}