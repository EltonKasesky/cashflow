namespace CashFlow.Application.UseCases.Expenses.Report;

internal static class ReportHelper
{
    public const string CURRENCY_SIMBOL = "R$";

    public static DateTime GetStartDate(DateOnly date)
    {
        return new DateTime(year: date.Year, month: date.Month, day: 1).Date.ToUniversalTime();
    }

    public static DateTime GetEndDate(DateOnly date)
    {
        int daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        return new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59).ToUniversalTime();
    }
}
