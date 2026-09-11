using CashFlow.Communication.Expense.Enums;

namespace CashFlow.Communication.Expense.Requests;

public class RequestExpense
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public EPaymentType PaymentType { get; set; }
}
