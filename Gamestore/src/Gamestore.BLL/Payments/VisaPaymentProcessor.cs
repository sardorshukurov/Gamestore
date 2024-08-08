using System.Text;
using Gamestore.BLL.DTOs.Order.Payment;
using Gamestore.BLL.Services.OrderService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Gamestore.BLL.Payments;

public class VisaPaymentProcessor(
    IOrderService orderService,
    HttpClient paymentClient,
    Guid customerId,
    ILogger logger,
    VisaModel? cardInfo)
    : PaymentProcessor(orderService, paymentClient, customerId, logger)
{
    protected override StringContent CreatePaymentRequest(Guid cartId, double cartSum)
    {
        // create request for the payment api
        var requestToPaymentApi = new VisaRequest(
            cartSum,
            cardInfo.Holder,
            cardInfo.CardNumber,
            cardInfo.MonthExpire,
            cardInfo.Cvv2,
            cardInfo.YearExpire);

        // serialize the request
        var serializedRequest = JsonConvert.SerializeObject(requestToPaymentApi);
        return new(serializedRequest, Encoding.UTF8, "application/json");
    }

    protected override string GetServiceUri() => "/visa";
}