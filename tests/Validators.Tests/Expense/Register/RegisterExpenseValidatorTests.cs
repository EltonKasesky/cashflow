using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Expense.Enums;
using CashFlow.Communication.Expense.Requests;
using CashFlow.Exception.Resources;
using CommonTestUtilities;
using FluentAssertions;
using FluentValidation.Results;

namespace Validators.Tests.Expense.Register;

public class RegisterExpenseValidatorTests
{
    [Fact]
    public void Success()
    {
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpense request = RequestRegisterExpenseBuilder.Build();

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title)
    {
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpense request = RequestRegisterExpenseBuilder.Build();

        request.Title = title;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Error_Amount_Invalid(decimal amount)
    {
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpense request = RequestRegisterExpenseBuilder.Build();

        request.Amount = amount;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }

    [Fact]
    public void Error_Date_In_Future()
    {
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpense request = RequestRegisterExpenseBuilder.Build();

        request.Date = DateTime.UtcNow.AddDays(+1);

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EXPENSE_CANNOT_FOR_THE_FUTURE));
    }

    [Fact]
    public void Error_Payment_Type_Invalid()
    {
        ExpenseValidator validator = new ExpenseValidator();
        RequestExpense request = RequestRegisterExpenseBuilder.Build();

        request.PaymentType = (EPaymentType)1000;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }
}
