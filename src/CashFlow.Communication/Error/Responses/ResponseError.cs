namespace CashFlow.Communication.Error.Responses;

public class ResponseError
{
    public List<string> ErrorMessages { get; set; }

    public ResponseError(string error) 
    {
        ErrorMessages = [error];
    }

    public ResponseError(List<string> errors)
    {
        ErrorMessages = errors;
    }
}
