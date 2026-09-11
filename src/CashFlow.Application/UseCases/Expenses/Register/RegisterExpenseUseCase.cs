using AutoMapper;
using CashFlow.Communication.Expense.Requests;
using CashFlow.Communication.Expense.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.Exceptions;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Expenses.Register;

internal class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesWriteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterExpenseUseCase(
        IExpensesWriteRepository repository, 
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisterExpense> Execute(RequestExpense request)
    {
        Validate(request);

        Expense expense = _mapper.Map<Expense>(request);

        await _repository.Add(expense);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisterExpense>(expense);
    }

    private void Validate(RequestExpense request)
    {
        ExpenseValidator validator = new ExpenseValidator();
        ValidationResult result = validator.Validate(request);

        if (!result.IsValid)
        {
            List<string> errors = result.Errors.Select(error => error.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
