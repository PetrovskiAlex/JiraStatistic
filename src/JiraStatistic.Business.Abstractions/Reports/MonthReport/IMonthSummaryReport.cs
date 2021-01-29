using System.Threading.Tasks;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public interface IMonthSummaryReport
    {
        Task MakeReport();
    }
}