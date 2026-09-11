using AutoMapper;
using CashFlow.Communication.Expense.Requests;
using CashFlow.Communication.Expense.Responses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestExpense, Expense>()
            .ForMember(destine => destine.Date, source => source.MapFrom(expense => expense.Date.ToUniversalTime()));
    }
    
    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpense>();
        CreateMap<Expense, ResponseShortExpense>();
        CreateMap<Expense, ResponseExpense>();
    }
}
