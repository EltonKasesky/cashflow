using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;

public interface IUsersWriteRepository
{
    Task Add(User user);
}
