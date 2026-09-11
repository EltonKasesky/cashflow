using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Communication.Error.Responses;
using CashFlow.Communication.Expense.Requests;
using CashFlow.Communication.Expense.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

public class ExpensesController : CashFlowBaseController
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseExpenses), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll([FromServices] IGetAllExpensesUseCase useCase)
    {
        ResponseExpenses response = await useCase.Execute();

        if (!response.Expenses.Any())
            return NoContent();

        return Ok(response);
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseExpense), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(
        [FromRoute] long id, 
        [FromServices] IGetExpenseByIdUseCase useCase
    )
    {
        ResponseExpense response = await useCase.Execute(id);
        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterExpense), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterExpenseUseCase useCase, 
        [FromBody] RequestExpense request
    ) 
    {
        ResponseRegisterExpense response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
        [FromRoute] long id,
        [FromServices] IUpdateExpenseUseCase useCase,
        [FromBody] RequestExpense request
    )
    {
        await useCase.Execute(id, request);
        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        [FromRoute] long id, 
        [FromServices] IDeleteExpenseUseCase useCase
    )
    {
        await useCase.Execute(id);
        return NoContent();
    }
}
