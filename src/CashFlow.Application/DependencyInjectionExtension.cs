using CashFlow.Application.AutoMapper;
using CashFlow.Application.Fonts;
using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Application.UseCases.Expenses.Register;
using CashFlow.Application.UseCases.Expenses.Report.Excel;
using CashFlow.Application.UseCases.Expenses.Report.Pdf;
using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Application.UseCases.Users.Register;
using Microsoft.Extensions.DependencyInjection;
using PdfSharp.Fonts;

namespace CashFlow.Application;

public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddExpensesUseCases(services);
        AddUsersUseCases(services);
        AddAutoMapper(services);
        AddFontResolver();
    }

    private static void AddExpensesUseCases(IServiceCollection services)
    {
        services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
        services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();
        services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        services.AddScoped<IGenerateExpensesExcelReportUseCase, GenerateExpensesExcelReportUseCase>();
        services.AddScoped<IGenerateExpensesPdfReportUseCase, GenerateExpensesPdfReportUseCase>();
    }

    private static void AddUsersUseCases(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(config => config.AddProfile<AutoMapping>());
    }

    private static void AddFontResolver()
    {
        GlobalFontSettings.FontResolver = new CashFlowFontResolver();
    }
}
