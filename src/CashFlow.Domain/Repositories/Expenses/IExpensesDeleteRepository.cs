using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpensesDeleteRepository
{
    /// <summary>
    ///     This function return TRUE if the deletion was successful otherwise, returns FALSE.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>True or False</returns>
    Task<bool> Delete(long id);
}
