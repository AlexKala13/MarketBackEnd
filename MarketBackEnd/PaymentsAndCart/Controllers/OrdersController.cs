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
        public async Task<ActionResult<ServiceResponse<List<GetOrderDTO>>>> GetAll(int userId)
        {
            return await _orderService.GetOrders(userId);
        }

        [HttpPost("Add")]
        public async Task<ActionResult<ServiceResponse<GetOrderDTO>>> AddCard(AddOrderDTO newOrder, int? debitCardId)
        {
            var response = await _orderService.AddOrder(newOrder, debitCardId);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
