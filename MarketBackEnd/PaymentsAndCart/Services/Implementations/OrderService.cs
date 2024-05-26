using AutoMapper;
using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.PaymentsAndCart.Models;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
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

        public OrderService(ApplicationDbContext db, IMapper mapper, IUserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ServiceResponse<GetOrderDTO>> AddOrder(AddOrderDTO newOrder)
        {
            var response = new ServiceResponse<GetOrderDTO>();
            try
            {
                if (newOrder == null)
                {
                    response.Success = false;
                    response.Message = "Order parameters are empty or null.";
                    return response;
                }
                var order = _mapper.Map<Orders>(newOrder);

                await _db.Orders.AddAsync(order);
                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<GetOrderDTO>(newOrder);
                response.Success = true;
                response.Message = "Order added successfully.";
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
                var orders = await _db.Orders.Where(x => x.BuyerId == userId || x.SellerId == userId).ToListAsync();
                var ordersDTOs = _mapper.Map<List<GetOrderDTO>>(orders);

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
