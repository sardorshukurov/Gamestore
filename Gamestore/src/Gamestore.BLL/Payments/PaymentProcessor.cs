using System.Net;
using System.Text;
using Gamestore.BLL.Services.OrderService;
using Microsoft.Extensions.Logging;

namespace Gamestore.BLL.Payments;

public abstract class PaymentProcessor(
    IOrderService orderService,
    HttpClient paymentClient,
    Guid customerId,
    ILogger logger)
{
    private readonly int _maxRetryAttempts = 3;

    public async Task<PaymentResult> ProcessPayment()
    {
        var cartId = await orderService.GetCartIdAsync(customerId);
        var cartSum = await orderService.GetCartSumAsync(customerId);
        var requestContent = CreatePaymentRequest(cartId, cartSum);
        return await AttemptPayment(cartId, cartSum, requestContent);
    }

    protected abstract StringContent CreatePaymentRequest(Guid cartId, double cartSum);

    protected async Task<PaymentResult> AttemptPayment(Guid cartId, double cartSum, StringContent requestContent)
    {
        var attempt = 0;
        var result = new HttpResponseMessage();

        while (attempt < _maxRetryAttempts)
        {
            result = await paymentClient.PostAsync(GetServiceUri(), requestContent);

            if (result.IsSuccessStatusCode)
            {
                await orderService.PayOrderAsync(cartId);
                return new PaymentResult(
                    Success: true,
                    Message: "Payment successful",
                    Amount: cartSum,
                    Date: DateTime.Now,
                    OrderId: cartId);
            }

            attempt++;
            logger.LogWarning($"Attempt for payment number {attempt}. Result: {await result.Content.ReadAsStringAsync()}");

            await Task.Delay(TimeSpan.FromSeconds(attempt * 2));
        }

        await orderService.CancelOrderAsync(cartId);

        var message = new StringBuilder($"Payment failed after: {attempt} attempts. ");
        if (result.StatusCode == HttpStatusCode.BadRequest)
        {
            message.Append(await result.Content.ReadAsStringAsync());
        }

        return new PaymentResult(
            Success: false,
            Message: message.ToString());
    }

    protected abstract string GetServiceUri();
}