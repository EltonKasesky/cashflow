using CashFlow.Application.UseCases.Login;
using CashFlow.Communication.Error.Responses;
using CashFlow.Communication.Login.Requests;
using CashFlow.Communication.Users.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.API.Controllers;

public class LoginController : CashFlowBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUser), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLogin request
    )
    {
        ResponseRegisteredUser response = await useCase.Execute(request);
        return Ok(response);
    }
}
