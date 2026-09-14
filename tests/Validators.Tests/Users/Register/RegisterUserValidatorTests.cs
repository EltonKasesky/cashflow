using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Communication.Users.Requests;
using CashFlow.Exception.Resources;
using CommonTestUtilities;
using FluentAssertions;
using FluentValidation.Results;

namespace Validators.Tests.Users.Register;

public class RegisterUserValidatorTests
{
    [Fact]
    public void Success()
    {
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Name_Empty(string? name)
    {
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

        request.Name = name;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

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
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

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
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

        request.Password = password;

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_REQUIRED));
    }

    [Fact]
    public void Error_Password_Must_Be_Greater()
    {
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

        request.Password = "!Cc1";

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_MUST_BE_GREATER_THAN_EIGHT));
    }

    [Fact]
    public void Error_Password_Invalid()
    {
        RequestRegisterUser request = RequestRegisterUserBuilder.Build();
        RegisterUserValidator validator = new RegisterUserValidator();

        request.Password = "A0123456789";

        ValidationResult result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And
            .Contain(error => error.ErrorMessage.Equals(ResourceErrorMessages.PASSWORD_INVALID));
    }
}
