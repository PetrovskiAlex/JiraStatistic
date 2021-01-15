using System.Threading.Tasks;
using JiraStatistic.Domain.Settings.Jira;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public interface IMonthSummaryReportDataProvider
    {
        Task<MonthSummaryReportData> GetData(JiraAuthSettings? authSettings = null);
    }
}