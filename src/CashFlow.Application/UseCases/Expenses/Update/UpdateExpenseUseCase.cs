using AutoMapper;
using CashFlow.Communication.Expense.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.Exceptions;
using CashFlow.Exception.Resources;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Expenses.Update;

internal class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpensesUpdateRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateExpenseUseCase(
        IExpensesUpdateRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Execute(long id, RequestExpense request)
    {
        Validate(request);

        Expense? expense = await _repository.GetById(id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);

        _repository.Update(expense);
        await _unitOfWork.Commit();
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
