using System.Threading.Tasks;
using JiraStatistic.Domain.Settings;
using JiraStatistic.Domain.Settings.Jira;
using JiraStatistic.Domain.Settings.Report;

namespace JiraStatistic.Business.Abstractions.Reports.MonthReport
{
    public interface IMonthSummaryReportDataProvider
    {
        Task<MonthSummaryReportData> GetData(MonthReportSummarySettings monthReportSettings, JiraProjectSettings projectSettings, JiraAuthSettings? authSettings = null);
    }
}