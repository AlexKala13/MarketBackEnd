using MarketBackEnd.Products.Advertisements.DTOs;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Shared.Model;
using Microsoft.AspNetCore.Mvc;

namespace MarketBackEnd.Products.Advertisements.Services.Interfaces
{
    public interface IAdvertisementService
    {
        Task<ServiceResponse<List<GetAdvertisementsDTO>>> GetAdvertisements(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status);
        Task<ServiceResponse<GetAdvertisementDTO>> GetAdvertisementById(int id);
        Task<ServiceResponse<GetAdvertisementDTO>> AddAdvertisement(int userId, NewAdvertisementDTO newAd);
        Task<ServiceResponse<GetAdvertisementDTO>> EditAdvertisement(int id, int userId, EditAdvertisementDTO updatedAd);
        Task<ServiceResponse<string>> DeleteAdvertisement(int id, int userId);
    }
}
