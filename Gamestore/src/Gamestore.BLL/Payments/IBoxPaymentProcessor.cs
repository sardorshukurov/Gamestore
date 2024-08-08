using System.Text;
using Gamestore.BLL.DTOs.Order.Payment;
using Gamestore.BLL.Services.OrderService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gamestore.BLL.Payments;

public class IBoxPaymentProcessor(
    IOrderService orderService,
    HttpClient paymentClient,
    Guid customerId,
    ILogger logger)
    : PaymentProcessor(orderService, paymentClient, customerId, logger)
{
    private readonly Guid _customerId = customerId;

    protected override StringContent CreatePaymentRequest(Guid cartId, double cartSum)
    {
        var requestToPaymentApi = new IBoxRequest(cartSum, _customerId, cartId);

        var serializedRequest = JsonConvert.SerializeObject(requestToPaymentApi);
        return new StringContent(serializedRequest, Encoding.UTF8, "application/json");
    }

    protected override string GetServiceUri() => "/ibox";
}