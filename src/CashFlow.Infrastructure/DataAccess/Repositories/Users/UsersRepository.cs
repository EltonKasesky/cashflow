using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories.Users;

internal class UsersRepository : IUsersReadRepository, IUsersWriteRepository
{
    private readonly CashFlowDbContext _dbContext;

    public UsersRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.User.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        return await _dbContext.User.AsNoTracking().AnyAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user)
    {
        await _dbContext.User.AddAsync(user);
    }
}
