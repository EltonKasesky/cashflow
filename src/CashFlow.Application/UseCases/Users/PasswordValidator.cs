

using CashFlow.Exception.Resources;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCases.Users;

public class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE = "ErrorMessage";

    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        return $"{{{ERROR_MESSAGE}}}";
    }

    public override bool IsValid(FluentValidation.ValidationContext<T> context, string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_REQUIRED);
            return false;
        }

        if (password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_MUST_BE_GREATER_THAN_EIGHT);
            return false;
        }

        Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[?!/*]).*$");
        if (!regex.IsMatch(password))
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE, ResourceErrorMessages.PASSWORD_INVALID);
            return false;
        }

        return true;
    }
}
