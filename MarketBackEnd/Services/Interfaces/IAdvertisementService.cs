using MarketBackEnd.DTOs.AdsWithPhoto;
using MarketBackEnd.Model;

namespace MarketBackEnd.Services.Interfaces
{
    public interface IAdvertisementService
    {
        Task<ServiceResponse<List<GetAdsWithPhotosDTO>>> GetAdvertisements(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status);
        Task<ServiceResponse<GetAdvertisementWithPhotosDTO>> GetAdvertisementById(int id);
    }
}
