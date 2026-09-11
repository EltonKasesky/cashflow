using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Expenses;

internal class ExpensesRepository : IExpensesReadRepository, IExpensesWriteRepository, 
    IExpensesUpdateRepository, IExpensesDeleteRepository
{
    private readonly CashFlowDbContext _dbContext;

    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbContext.Expense.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadRepository.GetById(long id)
    {
        return await _dbContext.Expense.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task<List<Expense>> FilterByMonth(DateTime startDate, DateTime endDate)
    {
        return await _dbContext.Expense
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }

    async Task<Expense?> IExpensesUpdateRepository.GetById(long id)
    {
        return await _dbContext.Expense.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public async Task Add(Expense expense)
    {
        await _dbContext.Expense.AddAsync(expense);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expense.Update(expense);
    }

    public async Task<bool> Delete(long id)
    {
        Expense? expense = await _dbContext.Expense.FirstOrDefaultAsync(expense => expense.Id == id);

        if (expense is null)
            return false;

        _dbContext.Expense.Remove(expense);

        return true;
    }
}
