using AutoMapper;
using CashFlow.Communication.Users.Requests;
using CashFlow.Communication.Users.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception.Exceptions;
using CashFlow.Exception.Resources;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.Register;

internal class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUsersReadRepository _readRepository;
    private readonly IUsersWriteRepository _writeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _tokenGenerator;
    private readonly IMapper _mapper;
    
    public RegisterUserUseCase(
        IUsersReadRepository readRepository,
        IUsersWriteRepository writeRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter,
        IAccessTokenGenerator tokenGenerator,
        IMapper mapper
    )
    {
        _readRepository = readRepository;
        _writeRepository = writeRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
        _tokenGenerator = tokenGenerator;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredUser> Execute(RequestRegisterUser request)
    {
        await Validate(request);

        User user = _mapper.Map<User>(request);

        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifier = Guid.NewGuid();

        await _writeRepository.Add(user);
        await _unitOfWork.Commit();

        return new ResponseRegisteredUser
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }

    private async Task Validate(RequestRegisterUser request)
    {
        ValidationResult result = new RegisterUserValidator().Validate(request);

        bool emailExists = await _readRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExists)
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
