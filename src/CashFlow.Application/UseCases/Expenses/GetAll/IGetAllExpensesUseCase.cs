using CashFlow.Communication.Expense.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

public interface IGetAllExpensesUseCase
{
    Task<ResponseExpenses> Execute();
}
