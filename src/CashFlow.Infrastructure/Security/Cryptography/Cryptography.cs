using CashFlow.Domain.Security.Cryptography;

namespace CashFlow.Infrastructure.Security.Cryptography;

internal class Cryptography : IPasswordEncripter
{
    public string Encrypt(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
