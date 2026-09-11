using AutoMapper;
using CashFlow.Communication.Expense.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.Exceptions;
using CashFlow.Exception.Resources;

namespace CashFlow.Application.UseCases.Expenses.GetById;

internal class GetExpenseByIdUseCase : IGetExpenseByIdUseCase
{
    private readonly IExpensesReadRepository _repository;
    private readonly IMapper _mapper;

    public GetExpenseByIdUseCase(IExpensesReadRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpense> Execute(long id)
    {
        Expense? expense = await _repository.GetById(id);

        Validate(expense);

        return _mapper.Map<ResponseExpense>(expense);
    }

    private void Validate(Expense? expense)
    {
        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }
}
