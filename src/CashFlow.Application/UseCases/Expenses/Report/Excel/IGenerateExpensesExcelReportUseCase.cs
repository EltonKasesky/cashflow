using CashFlow.Communication.Report.Requests;

namespace CashFlow.Application.UseCases.Expenses.Report.Excel;

public interface IGenerateExpensesExcelReportUseCase
{
    Task<byte[]> Execute(RequestInformationReport request);
}
