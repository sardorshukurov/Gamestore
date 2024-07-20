namespace Gamestore.API.DTOs.Order.Payment;

public record PaymentMethodResponse(
    string ImageUrl,
    string Title,
    string Description);

public static class PaymentMethodsHelper
{
    public static readonly List<PaymentMethodResponse> PaymentMethods =
    [
        new PaymentMethodResponse(
            "https://cdn-icons-png.flaticon.com/512/858/858170.png",
            "Bank",
            "Bank payment"),
        new PaymentMethodResponse(
            "https://cdn-icons-png.flaticon.com/512/11105/11105672.png",
            "IBox terminal",
            "IBox terminal payment"),
        new PaymentMethodResponse(
            "https://static-00.iconduck.com/assets.00/visa-icon-2048x628-6yzgq2vq.png",
            "Visa",
            "Visa payment")
    ];
}