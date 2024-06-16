using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.Shared.Model;

namespace MarketBackEnd.PaymentsAndCart.Services.Interfaces
{
    public interface IOrderService
    {
        Task<ServiceResponse<List<GetOrderDTO>>> GetOrders(int userId, int ordersType);
        Task<ServiceResponse<List<GetOrderDTO>>> AddOrders(List<AddOrderDTO> newOrders);
        Task<ServiceResponse<GetOrderDTO>> ConfirmOrder(int id, int? debitCardId);
        Task<ServiceResponse<GetOrderDTO>> ChangeOrderStatus(int id);
    }
}
