using CashFlow.Communication.Expense.Responses;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public interface IGetExpenseByIdUseCase
{
    Task<ResponseExpense> Execute(long id);
}
