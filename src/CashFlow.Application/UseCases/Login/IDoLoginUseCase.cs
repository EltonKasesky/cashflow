using CashFlow.Communication.Login.Requests;
using CashFlow.Communication.Users.Responses;

namespace CashFlow.Application.UseCases.Login;

public interface IDoLoginUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestLogin request);
}
