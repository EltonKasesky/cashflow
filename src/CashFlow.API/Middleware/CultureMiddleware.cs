using System.Globalization;

namespace CashFlow.API.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        List<CultureInfo> supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        string? requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        CultureInfo cultureInfo = new CultureInfo("en");

        if (!string.IsNullOrWhiteSpace(requestedCulture) &&
            supportedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
        {
            cultureInfo = new CultureInfo(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
