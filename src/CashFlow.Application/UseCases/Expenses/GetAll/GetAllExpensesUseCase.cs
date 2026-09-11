using AutoMapper;
using CashFlow.Communication.Expense.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCases.Expenses.GetAll;

internal class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpensesReadRepository _repository;
    private readonly IMapper _mapper;

    public GetAllExpensesUseCase(IExpensesReadRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseExpenses> Execute()
    {
        List<Expense> expenses = await _repository.GetAll();

        return new ResponseExpenses
        {
            Expenses = _mapper.Map<List<ResponseShortExpense>>(expenses)
        };
    }
}
