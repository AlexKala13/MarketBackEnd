using MarketBackEnd.PaymentsAndCart.DTOs.Orders;

namespace MarketBackEnd.PaymentsAndCart.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProductPurchase(int? debitCardId, int buyerId, int sellerId, decimal price);
        Task<bool> AdvertisementPayment(int userId, int? status);
    }
}