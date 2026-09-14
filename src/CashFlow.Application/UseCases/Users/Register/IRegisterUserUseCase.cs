using CashFlow.Communication.Users.Requests;
using CashFlow.Communication.Users.Responses;

namespace CashFlow.Application.UseCases.Users.Register;

public interface IRegisterUserUseCase
{
    Task<ResponseRegisteredUser> Execute(RequestRegisterUser request);
}
