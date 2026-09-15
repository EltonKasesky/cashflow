using Bogus;
using CashFlow.Communication.Login.Requests;

namespace CommonTestUtilities;

public static class RequestLoginBuilder
{
    public static RequestLogin Build()
    {
        Faker faker = new Faker();

        return new RequestLogin
        {
            Email = faker.Person.Email,
            Password = $"Ton{faker.Random.Number(1000, 9999)}{faker.PickRandom('?', '!', '/', '*')}"
        };
    }
}
