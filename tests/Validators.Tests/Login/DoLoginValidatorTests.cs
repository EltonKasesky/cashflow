using CashFlow.Application.UseCases.Login;
using CashFlow.Communication.Login.Requests;
using CashFlow.Exception.Resources;
using CommonTestUtilities;
using FluentAssertions;
using FluentValidation.Results;

namespace Validators.Tests.Login;

public class DoLoginValidatorTests
{
    [Fact]
    public void Success()
    {
        RequestLogin request = RequestLoginBuilder.Build();
        DoLoginValidator validator = new DoLoginValidator();

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        RequestLogin request = RequestLoginBuilder.Build();
        DoLoginValidator validator = new DoLoginValidator();

        request.Email = email;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_REQUIRED));
    }

    [Theory]
    [InlineData("@.com")]
    [InlineData(".com")]
    [InlineData("test.com")]
    public void Error_Email_Invalid(string email)
    {
        RequestLogin request = RequestLoginBuilder.Build();
        DoLoginValidator validator = new DoLoginValidator();

        request.Email = email;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData(null)]
    public void Error_Password_Empty(string password)
    {
        RequestLogin request = RequestLoginBuilder.Build();
        DoLoginValidator validator = new DoLoginValidator();

        request.Password = password;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_REQUIRED));
    }

    [Fact]
    public void Error_Password_Must_Be_Greater()
    {
        RequestLogin request = RequestLoginBuilder.Build();
        DoLoginValidator validator = new DoLoginValidator();

        request.Password = "!Cc1";

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_MUST_BE_GREATER_THAN_EIGHT));
    }

    [Fact]
    public void Error_Password_Invalid()
    {
        RequestLogin request = RequestLoginBuilder.Build();
        DoLoginValidator validator = new DoLoginValidator();

        request.Password = "A0123456789";

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_INVALID));
    }
}
