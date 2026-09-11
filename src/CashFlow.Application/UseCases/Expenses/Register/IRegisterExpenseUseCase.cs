using CashFlow.Communication.Expense.Requests;
using CashFlow.Communication.Expense.Responses;

namespace CashFlow.Application.UseCases.Expenses.Register;

public interface IRegisterExpenseUseCase
{
    Task<ResponseRegisterExpense> Execute(RequestExpense request);
}
