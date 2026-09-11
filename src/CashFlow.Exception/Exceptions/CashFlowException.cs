namespace CashFlow.Exception.Exceptions;

public abstract class CashFlowException : System.Exception
{
    public abstract int StatusCode { get; }
    
    protected CashFlowException() { }
    protected CashFlowException(string message) : base(message) { }

    public abstract List<string> GetErrors();
}
