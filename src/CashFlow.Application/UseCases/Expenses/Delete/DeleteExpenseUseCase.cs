using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.Exceptions;
using CashFlow.Exception.Resources;

namespace CashFlow.Application.UseCases.Expenses.Delete;

internal class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesDeleteRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseUseCase(IExpensesDeleteRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        bool deleted = await _repository.Delete(id);

        Validate(deleted);

        await _unitOfWork.Commit();
    }

    private void Validate(bool deleted)
    {
        if (!deleted)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);
    }
}
