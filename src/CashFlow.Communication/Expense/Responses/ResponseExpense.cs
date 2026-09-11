using CashFlow.Communication.Expense.Enums;

namespace CashFlow.Communication.Expense.Responses;

public class ResponseExpense
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public EPaymentType PaymentType { get; set; }
}
