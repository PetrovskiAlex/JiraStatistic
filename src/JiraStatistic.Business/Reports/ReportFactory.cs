using System;
using JiraStatistic.Business.Abstractions.Reports;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Business.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.DependencyInjection;

namespace JiraStatistic.Business.Reports
{
    public class ReportFactory : IReportFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ReportFactory(IServiceProvider serviceProvider )
        {
            _serviceProvider = serviceProvider;
        }
        
        public IReportSaver CreateReportSaver(DocumentTypeEnum documentType)
        {
            return documentType switch
            {
                DocumentTypeEnum.Excel => _serviceProvider.GetService<ExcelMonthReportSaver>() ?? throw new NullReferenceException(),
                _ => throw new ArgumentOutOfRangeException(nameof(documentType), documentType, null)
            };
        }
    }
}