namespace Gamestore.API.DTOs.Order.Payment;

public record PaymentRequest(
    string Method,
    VisaModel? Model);

public record VisaModel(
    string Holder,
    string CardNumber,
    short MonthExpire,
    short YearExpire,
    short Cvv2);