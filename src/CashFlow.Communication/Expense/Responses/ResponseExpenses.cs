namespace CashFlow.Communication.Expense.Responses;

public class ResponseExpenses
{
    public List<ResponseShortExpense> Expenses { get; set; } = new List<ResponseShortExpense>();
}
