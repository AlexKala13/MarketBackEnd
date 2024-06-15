using AutoMapper;
using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.PaymentsAndCart.Models;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
using MarketBackEnd.Products.Advertisements.Models;
using MarketBackEnd.Shared.Data;
using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Customer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.PaymentsAndCart.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IPaymentService _paymentService;

        public OrderService(ApplicationDbContext db, IMapper mapper, IUserService userService, IPaymentService paymentService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
            _paymentService = paymentService;
        }

        public async Task<ServiceResponse<List<GetOrderDTO>>> AddOrders(List<AddOrderDTO> newOrders)
        {
            var response = new ServiceResponse<List<GetOrderDTO>>();
            try
            {
                if (newOrders == null || !newOrders.Any())
                {
                    response.Success = false;
                    response.Message = "Order parameters are empty or null.";
                    return response;
                }

                //var paymentConfirm = await _paymentService.ProductPurchase(debitCardId, newOrders.First().BuyerId, newOrders.First().SellerId, newOrders.First().Price);

                //if (!paymentConfirm)
                //{
                //    response.Success = false;
                //    response.Message = "Failed to process payment.";
                //    return response;
                //}


                var orders = _mapper.Map<List<Orders>>(newOrders);
                orders.ForEach(order => order.Status = 1);

                await _db.Orders.AddRangeAsync(orders);
                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<List<GetOrderDTO>>(newOrders);
                response.Success = true;
                response.Message = "Orders added successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<GetOrderDTO>> ChangeOrderStatus(int id, int userId, int newStatus)
        {
            var response = new ServiceResponse<GetOrderDTO>();
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);

                bool isAdmin = await _userService.IsAdmin(userId);

                if ((userId != order.BuyerId && userId != order.SellerId) && !isAdmin)
                {
                    response.Success = false;
                    response.Message = "You are not authorized to change the status of this order.";
                    return response;
                }
                else if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                    return response;
                }
                else if (newStatus < 0 || newStatus > 2)
                {
                    response.Success = false;
                    response.Message = "Invalid order status.";
                    return response;
                }

                order.Status = newStatus;
                _db.Orders.Update(order);
                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<GetOrderDTO>(order);
                response.Success = true;
                response.Message = "Order status updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetOrderDTO>>> GetOrders(int userId)
        {
            var response = new ServiceResponse<List<GetOrderDTO>>();
            try
            {
                var orders = await _db.Orders
                    .Include(x => x.Buyer)
                    .Include(x => x.Seller)
                    .Include(x => x.Category)
                    .Where(x => x.BuyerId == userId || x.SellerId == userId)
                    .ToListAsync();

                var ordersDTOs = new List<GetOrderDTO>();

                foreach (var order in orders)
                {
                    var orderDTO = new GetOrderDTO
                    {
                        Id = order.Id,
                        BuyerId = order.BuyerId,
                        BuyerName = order.Buyer != null ? order.Buyer.UserName : "Unknown",
                        SellerId = order.SellerId,
                        SellerName = order.Seller != null ? order.Seller.UserName : "Unknown",
                        AdvertisementId = order.AdvertisementId,
                        Status = order.Status,
                        CategoryId = order.CategoryId,
                        CategoryName = order.Category != null ? order.Category.CategoryName : "Unknown",
                        Price = order.Price,
                        OrderDate = order.OrderDate,
                        PurchaseDate = order.PurchaseDate
                    };

                    ordersDTOs.Add(orderDTO);
                }

                response.Data = ordersDTOs;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }
    }
}
