namespace Gamestore.BLL.Payments;

public record PaymentResult(
    bool Success,
    string Message,
    double? Amount = null,
    DateTime? Date = null,
    Guid? OrderId = null);