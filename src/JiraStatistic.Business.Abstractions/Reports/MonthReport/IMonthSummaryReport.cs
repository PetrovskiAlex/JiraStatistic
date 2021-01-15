using System.Threading.Tasks;
using JiraStatistic.Domain.Settings.Report;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public interface IMonthSummaryReport
    {
        Task MakeReport(MonthSummarySettings settings);
    }
}