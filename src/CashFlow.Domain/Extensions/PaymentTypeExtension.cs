using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions;

public static class PaymentTypeExtension
{
    public static string PaymentTypeToString(this EPaymentType payment)
    {
        return payment switch
        {
            EPaymentType.CASH => ResourceReportGenerationMessages.CASH,
            EPaymentType.CREDIT_CARD => ResourceReportGenerationMessages.CREDIT_CARD,
            EPaymentType.DEBIT_CARD => ResourceReportGenerationMessages.DEBIT_CARD,
            EPaymentType.PIX => ResourceReportGenerationMessages.PIX,
            EPaymentType.ELETRONIC_TRANSFER => ResourceReportGenerationMessages.ELETRONIC_TRANSFER,
            _ => string.Empty
        };
    }
}
