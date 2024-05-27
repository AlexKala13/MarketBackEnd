using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.Shared.Model;

namespace MarketBackEnd.PaymentsAndCart.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<GetOrderDTO>>> GetOrders(int userId);
        Task<ServiceResponse<GetOrderDTO>> AddOrder(AddOrderDTO newOrder, int? debitCardId);
        Task<ServiceResponse<GetOrderDTO>> ChangeOrderStatus(int id, int userId, int newStatus);
    }
}
