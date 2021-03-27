using System.Threading.Tasks;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public interface IReportSaver
    {
        Task Save(ProjectSummaryReportData reportData);
    }
}