using CashFlow.Exception.Resources;
using System.Net;

namespace CashFlow.Exception.Exceptions;

public class InvalidLoginException : CashFlowException
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public InvalidLoginException() : base(ResourceErrorMessages.LOGIN_INVALID) { }

    public override List<string> GetErrors()
    {
        return new List<string>() { Message };
    }
}
