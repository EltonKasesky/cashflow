using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess;

internal class CashFlowDbContext : DbContext
{
    public DbSet<Expense> Expense { get; set; }
    public DbSet<User> User { get; set; }

    public CashFlowDbContext(DbContextOptions options) : base(options) { }
}
