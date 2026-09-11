using CashFlow.Application.UseCases.Expenses.Report.Excel;
using CashFlow.Application.UseCases.Expenses.Report.Pdf;
using CashFlow.Communication.Report.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace CashFlow.API.Controllers;

public class ReportController : CashFlowBaseController
{
    [HttpPost("excel")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetExcel(
        [FromServices] IGenerateExpensesExcelReportUseCase useCase,
        [FromBody] RequestInformationReport request
    )
    {
        byte[] file = await useCase.Execute(request);

        if (file.Length == 0)
            return NoContent();

        return File(file, MediaTypeNames.Application.Octet, "report.xlsx");
    }

    [HttpPost("pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetPdf(
        [FromServices] IGenerateExpensesPdfReportUseCase useCase,
        [FromBody] RequestInformationReport request
    )
    {
        byte[] file = await useCase.Execute(request);

        if (file.Length == 0)
            return NoContent();

        return File(file, MediaTypeNames.Application.Pdf, "report.pdf");
    }
}
