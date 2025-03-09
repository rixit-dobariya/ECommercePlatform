using ECommercePlatform.Models;
using Razorpay.Api;
namespace ECommercePlatform.Helpers.PaymentHelper
{
    public class PaymentService : IPaymentService
    {
        IConfiguration _configuration;
        public PaymentService(IConfiguration configuration) 
        {
            _configuration= configuration;
        }
        public Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest)
        {
            try
            {
                // Generate random receipt number for order
                Random randomObj = new Random();
                string transactionId = randomObj.Next(10000000, 100000000).ToString();

                string razorpayKey = _configuration["Razorpay:Key"];
                string razorpaySecret = _configuration["Razorpay:Secret"];
                RazorpayClient client = new RazorpayClient(razorpayKey, razorpaySecret);

                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payRequest.Amount * 100);
                options.Add("receipt", transactionId);
                options.Add("currency", "INR");
                options.Add("payment_capture", "0"); // 1 - automatic, 0 - manual
                                                     // options.Add("notes", "Test Payment of Merchant");

                Razorpay.Api.Order orderResponse = client.Order.Create(options); string orderId = orderResponse["id"].ToString();
                MerchantOrder order = new MerchantOrder
                {
                    OrderId = orderResponse.Attributes["id"],
                    RazorpayKey = razorpayKey,
                    Amount =(int)payRequest.Amount * 100,
                    Currency = "INR",
                    Name = payRequest.Name,
                    Email = payRequest.Email,
                    PhoneNumber = payRequest.PhoneNumber,
                    Address = payRequest.Address,
                    Description = "Order by Merchant"
                };

                return Task.FromResult(order);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor)
        {
            try
            {
                string paymentId = _httpContextAccessor.HttpContext.Request.Form["rzp_paymentid"];
                // This is orderId
                string orderId = _httpContextAccessor.HttpContext.Request.Form["rzp_orderid"];

                string razorpayKey = _configuration["Razorpay:Key"];
                string razorpaySecret = _configuration["Razorpay:Secret"];
                RazorpayClient client = new RazorpayClient(razorpayKey, razorpaySecret);
                Payment payment = client.Payment.Fetch(paymentId);

                // This code is for capturing the payment
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);

                Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];

                return paymentCaptured.Attributes["status"];
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}