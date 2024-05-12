using MarketBackEnd.DTOs.AdsWithPhoto;
using MarketBackEnd.Model;
using MarketBackEnd.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.Controllers
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
        public async Task<ActionResult<ServiceResponse<GetAdvertisementWithPhotosDTO>>> GetSingle(int id)
        {
            return await _advertisementService.GetAdvertisementById(id);
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<GetAdsWithPhotosDTO>>>> GetAll(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status)
        {
            return await _advertisementService.GetAdvertisements(name, categoryId, priceMin, priceMax, postDate, status);
        }
    }
}
