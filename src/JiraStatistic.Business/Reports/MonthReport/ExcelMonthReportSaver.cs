using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using JiraStatistic.Business.Abstractions.Reports.MonthReport;
using JiraStatistic.Domain.Settings.Report;
using Microsoft.Extensions.Options;

namespace JiraStatistic.Business.Reports.MonthReport
{
    public class ExcelMonthReportSaver : IReportSaver
    {
        private readonly ReportSettings _reportSettings;

        public ExcelMonthReportSaver(IOptions<ReportSettings> reportSettings)
        {
            _reportSettings = reportSettings.Value;
        }
        
        public async Task Save(ProjectSummaryReportData reportData)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Часы");
            
            ws.Cell(1,1).Value = "Имя и фамилия:";
            //ws.Cell(1,2).Value = reportData.Name; //TODO
            
            ws.Cell(2,1).Value = "Месяц:";
            //ws.Cell(2,2).Value = reportData.Date; //TODO
            ws.Cell(2,2).Style.DateFormat.Format = "MM.yyyy";
            
            ws.Cell(3,1).Value = "Запланированных часов:";
            ws.Cell(4,1).Value = "Закрытых часов:";
            ws.Cell(4,2).Value = reportData.ClosedHours;
            
            ws.Cell(6,1).Value = "Проект:";
            //ws.Cell(6,2).Value = reportData.Project.Name; //TODO
            
            ws.Cell(7,1).Value = "Часы:";
            //ws.Cell(7,2).Value = reportData.Project.ClosedHours; //TODO
            
            ws.Cell(8,1).Value = "Название задачи";
            ws.Cell(8,2).Value = "Часы";
            
            /*ws.Cell(9, 1).Value = reportData.Project.Tasks.Select(x => new {Name = $"{x.Code} {x.Name}", Hours = x.Hours}).AsEnumerable(); //TODO
            var rngTable = ws.Range(8,1,8 + reportData.Project.Tasks.Length,2);
            rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;*/
            
            var rngTitle1 = ws.Range(1,1,4,1);
            rngTitle1.Style
                .Font.SetBold()
                .Fill.BackgroundColor = XLColor.Silver;
            
            var rngTitle2 = ws.Range(6,1,7,1);
            rngTitle2.Style
                .Font.SetBold()
                .Fill.BackgroundColor = XLColor.Silver;
            
            var rngTitleTable = ws.Range(8,1,8,2);
            rngTitleTable.Style
                .Font.SetBold()
                .Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            rngTitleTable.Style.Fill.BackgroundColor = XLColor.Silver;

            ws.Columns().AdjustToContents();
            ws.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

            await Task.Run(() => wb.SaveAs($"{_reportSettings?.ReportSummary.Name} {_reportSettings?.ReportSummary.Year}_{_reportSettings?.ReportSummary.Month}.xlsx"));
        }
    }
}