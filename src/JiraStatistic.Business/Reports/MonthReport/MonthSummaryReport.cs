using System.Threading.Tasks;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Report;
using JiraStatistic.JiraClient.Clients.Search;

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class MonthSummaryReport : IMonthSummaryReport
    {
        private readonly IJiraSearchClient _jiraSearchClient;

        public MonthSummaryReport(IJiraSearchClient jiraSearchClient)
        {
            _jiraSearchClient = jiraSearchClient;
        }
        
        public Task MakeReport(MonthSummarySettings settings)
        {
            //GetData
            //Make a report
            //Save report

            throw new System.NotImplementedException();
        }
    }
}