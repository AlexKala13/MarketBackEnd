using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Products.Advertisements.Services.Interfaces;
using MarketBackEnd.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.Products.Advertisements.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _advertisementService;

        public AdvertisementController(IAdvertisementService advertisementService)
        {
            _advertisementService = advertisementService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<GetAdvertisementDTO>>> GetSingle(int id)
        {
            return await _advertisementService.GetAdvertisementById(id);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetAdvertisementsDTO>>>> GetAll(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status)
        {
            return await _advertisementService.GetAdvertisements(name, categoryId, priceMin, priceMax, postDate, status);
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> AddAdvertisement([FromBody] CreateAdvertisementDTO newAd)
        {
            var response = await _advertisementService.AddAdvertisement(newAd);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("Edit/{id}")]
        public async Task<ActionResult<ServiceResponse<GetAdvertisementDTO>>> UpdateAdvertisement(int id, EditAdvertisementDTO updatedAd)
        {
            return await _advertisementService.EditAdvertisement(id, updatedAd);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteAdvertisement(int id)
        {
            return await _advertisementService.DeleteAdvertisement(id);
        }
    }
}
