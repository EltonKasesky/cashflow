using Bogus;
using CashFlow.Communication.Expense.Enums;
using CashFlow.Communication.Expense.Requests;

namespace CommonTestUtilities;

public static class RequestRegisterExpenseBuilder
{
    public static RequestExpense Build()
    {
        Faker faker = new Faker();

        return new RequestExpense
        {
            Title = faker.Commerce.ProductName(),
            Description = faker.Commerce.ProductDescription(),
            Amount = faker.Random.Decimal(min: 1, max: 1000),
            Date = faker.Date.Past(),
            PaymentType = faker.PickRandom<EPaymentType>()
        };
    }
}
