using AutoMapper;
using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.PaymentsAndCart.Models;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
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

        public async Task<ServiceResponse<GetOrderDTO>> ChangeOrderStatus(int id)
        {
            var response = new ServiceResponse<GetOrderDTO>();
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);

                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                    return response;
                }

                order.Status = 2;
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

        public async Task<ServiceResponse<GetOrderDTO>> ConfirmOrder(int id, int? debitCardId)
        {
            var response = new ServiceResponse<GetOrderDTO>();
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == id);

                if (order == null)
                {
                    response.Success = false;
                    response.Message = "Order not found.";
                    return response;
                }

                var paymentConfirm = await _paymentService.ProductPurchase(debitCardId, order.BuyerId, order.SellerId, order.Price);

                if (!paymentConfirm)
                {
                    response.Success = false;
                    response.Message = "Failed to process payment.";
                    return response;
                }

                order.Status = 3;

                var advertisement = await _db.Advertisements.FirstOrDefaultAsync(x => x.Id == order.AdvertisementId);
                advertisement.Status = 1;

                var confirmedOrderDTO = _mapper.Map<Orders>(order);
                
                _db.Orders.Update(confirmedOrderDTO);
                _db.Advertisements.Update(advertisement);
                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<GetOrderDTO>(order);
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

        public async Task<ServiceResponse<List<GetOrderDTO>>> GetOrders(int userId, int ordersType)
        {
            var response = new ServiceResponse<List<GetOrderDTO>>();
            try
            {
                var query = _db.Orders.AsQueryable();

                if (ordersType == 0)
                {
                    query = query.Where(x => x.BuyerId == userId);
                }
                else
                {
                    query = query.Where(x => x.SellerId == userId);
                }

                query = query
                    .Include(x => x.Buyer)
                    .Include(x => x.Seller)
                    .Include(x => x.Category);

                var orders = await query.ToListAsync();

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
