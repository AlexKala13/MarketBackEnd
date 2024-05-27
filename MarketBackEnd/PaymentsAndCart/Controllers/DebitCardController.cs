using MarketBackEnd.PaymentsAndCart.DTOs.DebitCards;
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
    public class DebitCardController : ControllerBase
    {
        private readonly ICardService _paymentService;

        public DebitCardController(ICardService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetDebitCardDTO>>>> GetAll(int userId)
        {
            return await _paymentService.GetDebitCards(userId);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddAdvertisement(AddDebitCardDTO newCard)
        {
            var response = await _paymentService.AddDebitCard(newCard);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteAdvertisement(int id, int userId)
        {
            return await _paymentService.DeleteDebitCard(id, userId);
        }
    }
}
