using Bogus;
using CashFlow.Communication.Users.Requests;

namespace CommonTestUtilities;

public static class RequestRegisterUserBuilder
{
    public static RequestRegisterUser Build()
    {
        Faker faker = new Faker();

        return new RequestRegisterUser
        {
            Name = faker.Person.FullName,
            Email = faker.Person.Email,
            Password = $"Cashflow{faker.Random.Number(1000, 9999)}{faker.PickRandom('?', '!', '/', '*')}"
        };
    }
}
