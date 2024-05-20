using MarketBackEnd.DTOs.AdsWithPhoto;
using MarketBackEnd.DTOs.Advertisement;
using MarketBackEnd.Model;

namespace MarketBackEnd.Services.Interfaces
{
    public interface IAdvertisementService
    {
        Task<ServiceResponse<List<GetAdvertisementsDTO>>> GetAdvertisements(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status);
        Task<ServiceResponse<GetAdvertisementDTO>> GetAdvertisementById(int id);
        //Task<ServiceResponse<GetAdvertisementDTO>> AddAdvertisement(CreateAdvertisementDTO newAd);
    }
}
