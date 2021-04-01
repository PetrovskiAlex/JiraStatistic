using System.Globalization;
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
        
        public async Task Save(SummaryReportData summaryReportData)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Часы");
            ws.Column(2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
            
            ws.Cell(1,1).Value = "Имя и фамилия:";
            //ws.Cell(1,2).Value = reportData.Name; //TODO
            
            ws.Cell(2,1).Value = "Месяц:";
            ws.Cell(2,2).Value = summaryReportData.Date;
            ws.Cell(2,2).Style.DateFormat.Format = "MM.yyyy";
            
            ws.Cell(3,1).Value = "Запланированных часов:";
            ws.Cell(4,1).Value = "Закрытых часов:";
            ws.Cell(4,2).Value = summaryReportData.ClosedHours;
            
            var rngTitle1 = ws.Range(1,1,4,1);
            SetRangeStyle(rngTitle1);

            var row = 5;
            foreach (var reportData in summaryReportData.Projects)
            {
                ws.Cell(++row,1).Value = "Проект:";
                ws.Cell(row,2).Value = reportData.Name;
                SetRangeStyle(ws.Range(row, 1, row, 1));
            
                ws.Cell(++row,1).Value = "Часы:";
                ws.Cell(row,2).Value = reportData.ClosedHours;
                SetRangeStyle(ws.Range(row, 1, row, 1));
            
                ws.Cell(++row,1).Value = "Название задачи";
                ws.Cell(row,2).Value = "Часы";
                SetRangeStyle(ws.Range(row, 1, row, 2), XLAlignmentHorizontalValues.Center);
                
                var rngTable = ws.Range(row,1,row + reportData.Tasks.Length,2);
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rngTable.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                ws.Cell(++row, 1).Value = reportData.Tasks.Select(x => new
                    {
                        Name = $"{x.Code} {x.Name}", Hours = double.Parse(x.Hours.ToString(CultureInfo.CurrentCulture))
                    })
                    .AsEnumerable();
                
                row += reportData.Tasks.Length;
            }

            ws.Columns().AdjustToContents();

            await Task.Run(() => wb.SaveAs($"{_reportSettings.ReportSummary.Name} {_reportSettings.ReportSummary.Year}_{_reportSettings.ReportSummary.Month}.xlsx"));
        }

        private static void SetRangeStyle(IXLRangeBase range, XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left)
        {
            range.Style
                .Font.SetBold()
                .Alignment.Horizontal = alignmentHorizontal;
            range.Style.Fill.BackgroundColor = XLColor.Silver;
        }
    }
}