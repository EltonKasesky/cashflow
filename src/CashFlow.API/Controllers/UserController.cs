using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Error.Responses;
using CashFlow.Communication.Users.Requests;
using CashFlow.Communication.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

public class UserController : CashFlowBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUser request
    )
    {
        ResponseRegisteredUser response = await useCase.Execute(request);
        return Created(string.Empty, response);
    }
}
