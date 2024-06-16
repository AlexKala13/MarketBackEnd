using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.PaymentsAndCart.Services.Implementations;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
using MarketBackEnd.Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.PaymentsAndCart.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDTO>>>> GetAll(int userId, int ordersType)
        {
            return await _orderService.GetOrders(userId, ordersType);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ServiceResponse<List<GetOrderDTO>>>> AddOrder(List<AddOrderDTO> newOrders)
        {
            var response = await _orderService.AddOrders(newOrders);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("Confirm")]
        public async Task<ActionResult<ServiceResponse<GetOrderDTO>>> ConfirmOrder(int orderId, int? debitCardId)
        {
            var response = await _orderService.ConfirmOrder(orderId, debitCardId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("ConfirmBySeller")]
        public async Task<ActionResult<ServiceResponse<GetOrderDTO>>> ConfirmBySeller(int orderId)
        {
            var response = await _orderService.ChangeOrderStatus(orderId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
