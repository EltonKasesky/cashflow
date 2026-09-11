using CashFlow.Communication.Report.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using ClosedXML.Excel;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel;

internal class GenerateExpensesExcelReportUseCase : IGenerateExpensesExcelReportUseCase
{
    private readonly IExpensesReadRepository _repository;

    public GenerateExpensesExcelReportUseCase(IExpensesReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(RequestInformationReport request)
    {
        DateTime startDate = ReportHelper.GetStartDate(request.Month);
        DateTime endDate = ReportHelper.GetEndDate(request.Month);

        List<Expense> expenses = await _repository.FilterByMonth(startDate, endDate);

        if (!expenses.Any())
            return [];
        
        using XLWorkbook workbook = new XLWorkbook();

        ConfigWorkbook(workbook);

        IXLWorksheet worksheet = workbook.Worksheets.Add(request.Month.ToString("Y"));

        InsertHeader(worksheet);
        InsertExpenses(worksheet, expenses);
        
        worksheet.Columns().AdjustToContents();

        using MemoryStream file = new MemoryStream();
        workbook.SaveAs(file);

        return file.ToArray();
    }

    private void ConfigWorkbook(XLWorkbook workbook)
    {
        workbook.Author = "Elton Kasesky";
        workbook.Style.Font.FontSize = 12;
        workbook.Style.Font.FontName = "Arial";
    }

    private void InsertHeader(IXLWorksheet worksheet)
    {
        worksheet.Cell("A1").Value = ResourceReportGenerationMessages.TITLE;
        worksheet.Cell("B1").Value = ResourceReportGenerationMessages.DATE;
        worksheet.Cell("C1").Value = ResourceReportGenerationMessages.PAYMENT_TYPE;
        worksheet.Cell("D1").Value = ResourceReportGenerationMessages.AMOUNT;
        worksheet.Cell("E1").Value = ResourceReportGenerationMessages.DESCRIPTION;

        worksheet.Cells("A1:E1").Style.Font.Bold = true;
        worksheet.Cells("A1:E1").Style.Fill.BackgroundColor = XLColor.FromHtml("#F5C2B6");
         
        worksheet.Cells("A1:C1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        worksheet.Cell("D1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        worksheet.Cell("E1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
    }

    private void InsertExpenses(IXLWorksheet worksheet, List<Expense> expenses)
    {
        int raw = 2;

        expenses.ForEach(expense =>
        {
            worksheet.Cell($"A{raw}").Value = expense.Title;
            worksheet.Cell($"B{raw}").Value = expense.Date;
            worksheet.Cell($"C{raw}").Value = expense.PaymentType.PaymentTypeToString();

            worksheet.Cell($"D{raw}").Value = expense.Amount;
            worksheet.Cell($"D{raw}").Style.NumberFormat.Format = $"-{ReportHelper.CURRENCY_SIMBOL} #,##0.00";

            worksheet.Cell($"E{raw}").Value = expense.Description;

            raw++;
        });
    }
}
