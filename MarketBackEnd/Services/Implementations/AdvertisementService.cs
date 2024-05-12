using AutoMapper;
using MarketBackEnd.Data;
using MarketBackEnd.DTOs.AdsWithPhoto;
using MarketBackEnd.DTOs.Advertisement;
using MarketBackEnd.DTOs.Photo;
using MarketBackEnd.Model;
using MarketBackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.Services.Implementations
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public AdvertisementService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetAdvertisementWithPhotosDTO>> GetAdvertisementById(int id)
        {
            var serviceResponse = new ServiceResponse<GetAdvertisementWithPhotosDTO>();
            try
            {
                var advertisement = await _db.Advertisements.FirstOrDefaultAsync(x => x.Id == id);
                var photos = await _db.Photos.Where(x => x.AdvertisementId == id).ToListAsync();

                if (advertisement != null)
                {
                    var advertisementDTO = _mapper.Map<GetAdvertisementDTO>(advertisement);
                    var photosDTO = _mapper.Map<List<GetPhotoDTO>>(photos);

                    var advertisementWithPhotosDTO = new GetAdvertisementWithPhotosDTO
                    {
                        AdverisementInfo = advertisementDTO,
                        Photos = photosDTO
                    };

                    serviceResponse.Data = advertisementWithPhotosDTO;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Adverisement not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Adverisement not found " + ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAdsWithPhotosDTO>>> GetAdvertisements(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status)
        {
            var serviceResponse = new ServiceResponse<List<GetAdsWithPhotosDTO>>();
            try
            {
                var query = _db.Advertisements.AsQueryable();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(x => x.Name.Contains(name));
                }
                if (categoryId.HasValue)
                {
                    query = query.Where(x => x.CategoryId == categoryId.Value);
                }
                if (priceMin.HasValue)
                {
                    query = query.Where(x => x.Price >= priceMin.Value);
                }
                if (priceMax.HasValue)
                {
                    query = query.Where(x => x.Price <= priceMax.Value);
                }
                if (postDate.HasValue)
                {
                    query = query.Where(x => x.PostDate.Date == postDate.Value.Date);
                }
                if (status.HasValue)
                {
                    query = query.Where(x => x.Status == status.Value);
                }

                var advertisements = await query.ToListAsync();
                var photos = await _db.Photos.Where(x => x.IsMain == true).ToListAsync();

                if (advertisements != null)
                {
                    var advertisementsDTO = _mapper.Map<List<GetAdvertisementsDTO>>(advertisements);
                    var photosDTO = _mapper.Map<List<GetPhotoDTO>>(photos);

                    var advertisementsWithPhotosDTO = new GetAdsWithPhotosDTO
                    {
                        Advertisements = advertisementsDTO,
                        Photos = photosDTO
                    };

                    serviceResponse.Data = new List<GetAdsWithPhotosDTO> { advertisementsWithPhotosDTO };
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Advertisements not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to get advertisements: " + ex.Message;
            }
            return serviceResponse;
        }

    }
}
