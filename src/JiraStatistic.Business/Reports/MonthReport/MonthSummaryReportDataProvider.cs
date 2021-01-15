using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Jira;

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReportDataProvider : IMonthSummaryReportDataProvider
    {
        public Task<MonthSummaryReportData> GetData(JiraAuthSettings? authSettings = null)
        {
            throw new System.NotImplementedException();
        }
    }
}