using ECommercePlatform.Models;
namespace ECommercePlatform.Helpers.PaymentHelper
{
    public interface IPaymentService
    {
        Task<MerchantOrder> ProcessMerchantOrder(PaymentRequest payRequest);
        Task<string> CompleteOrderProcess(IHttpContextAccessor _httpContextAccessor);
    }

}
