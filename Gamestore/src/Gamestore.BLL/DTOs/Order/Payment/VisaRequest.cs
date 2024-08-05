namespace Gamestore.BLL.DTOs.Order.Payment;

public record VisaRequest(
    double TransactionAmount,
    string CardHolderName,
    string CardNumber,
    short ExpirationMonth,
    short Cvv,
    short ExpirationYear);