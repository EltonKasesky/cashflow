using CashFlow.Communication.Report.Requests;

namespace CashFlow.Application.UseCases.Expenses.Report.Pdf;

public interface IGenerateExpensesPdfReportUseCase
{
    Task<byte[]> Execute(RequestInformationReport request);
}
