using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Login.Requests;
using CashFlow.Exception.Resources;
using FluentValidation;

namespace CashFlow.Application.UseCases.Login;

public class DoLoginValidator : AbstractValidator<RequestLogin>
{
    public DoLoginValidator()
    {
        RuleFor(login => login.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_REQUIRED)
            .EmailAddress()
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
        RuleFor(login => login.Password).SetValidator(new PasswordValidator<RequestLogin>());
    }
}
