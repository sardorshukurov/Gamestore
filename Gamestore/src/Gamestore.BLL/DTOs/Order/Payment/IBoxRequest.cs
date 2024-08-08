namespace Gamestore.BLL.DTOs.Order.Payment;

public record IBoxRequest(
    double TransactionAmount,
    Guid AccountNumber,
    Guid InvoiceNumber);