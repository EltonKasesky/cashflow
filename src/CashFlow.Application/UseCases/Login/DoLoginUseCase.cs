using CashFlow.Communication.Login.Requests;
using CashFlow.Communication.Users.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.Exceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Login;

internal class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUsersReadRepository _repository;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IPasswordEncripter _passwordEncripter;

    public DoLoginUseCase(
        IUsersReadRepository repository,
        IAccessTokenGenerator tokenGenerator,
        IPasswordEncripter passwordEncripter
    )
    {
        _repository = repository;
        _tokenGenerator = tokenGenerator;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestLogin request)
    {
        Validate(request);

        User? user = await _repository.GetUserByEmail(request.Email)
            ?? throw new InvalidLoginException();

        bool passwordMatch = _passwordEncripter.Verify(request.Password, user.Password);

        if (!passwordMatch)
            throw new InvalidLoginException();

        return new ResponseRegisteredUser
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private void Validate(RequestLogin request)
    {
        ValidationResult result = new DoLoginValidator().Validate(request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
