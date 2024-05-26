using AutoMapper;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Products.Advertisements.Models;
using MarketBackEnd.Products.Advertisements.Services.Interfaces;
using MarketBackEnd.Shared.Data;
using MarketBackEnd.Shared.Model;
using MarketBackEnd.Users.Customer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.Products.Advertisements.Services.Implementations
{
    public class AdvertisementService : IAdvertisementService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AdvertisementService(ApplicationDbContext db, IMapper mapper, IUserService userService)
        {
            _db = db;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<ServiceResponse<GetAdvertisementDTO>> AddAdvertisement(int userId, CreateAdvertisementDTO newAd)
        {
            var serviceResponse = new ServiceResponse<GetAdvertisementDTO>();
            var advertisement = _mapper.Map<Advertisement>(newAd);
            advertisement.UserId = userId;

            if (newAd.Photos != null && newAd.Photos.Count > 0)
            {
                advertisement.Photos = new List<Photos>();
                foreach (var photoBytes in newAd.Photos)
                {
                    var photo = new Photos
                    {
                        Image = photoBytes,
                        IsMain = false
                    };
                    advertisement.Photos.Add(photo);
                }

                if (advertisement.Photos.Count > 0)
                {
                    advertisement.Photos.ElementAt(0).IsMain = true;
                }
            }

            _db.Advertisements.Add(advertisement);
            await _db.SaveChangesAsync();

            serviceResponse.Data = _mapper.Map<GetAdvertisementDTO>(advertisement);
            return serviceResponse;
        }

        public async Task<ServiceResponse<string>> DeleteAdvertisement(int id, int userId)
        {
            var serviceResponse = new ServiceResponse<string>();
            try
            {
                var advertisement = await _db.Advertisements.Include(a => a.Photos).FirstOrDefaultAsync(a => a.Id == id);
                if (advertisement == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Advertisement not found.";
                    return serviceResponse;
                }

                bool isAuthor = _userService.IsAuthor(userId, advertisement.UserId);
                bool isAdmin = await _userService.IsAdmin(userId);

                if (!isAuthor && !isAdmin)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Access denied.";
                    return serviceResponse;
                }

                if (advertisement.Photos != null && advertisement.Photos.Any())
                {
                    _db.Photos.RemoveRange(advertisement.Photos);
                }

                _db.Advertisements.Remove(advertisement);
                await _db.SaveChangesAsync();

                serviceResponse.Data = "Advertisement deleted successfully.";
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to delete advertisement: " + ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAdvertisementDTO>> EditAdvertisement(int id, int userId, EditAdvertisementDTO updatedAd)
        {
            var serviceResponse = new ServiceResponse<GetAdvertisementDTO>();
            try
            {
                var advertisement = await _db.Advertisements.Include(a => a.Photos).FirstOrDefaultAsync(a => a.Id == id);
                if (advertisement == null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Advertisement not found.";
                    return serviceResponse;
                }

                bool isAuthor = _userService.IsAuthor(userId, advertisement.UserId);
                bool isAdmin = await _userService.IsAdmin(userId);

                if (!isAuthor && !isAdmin)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Access denied.";
                    return serviceResponse;
                }

                advertisement.Name = updatedAd.Name ?? advertisement.Name;
                advertisement.Description = updatedAd.Description ?? advertisement.Description;
                advertisement.CategoryId = updatedAd.CategoryId ?? advertisement.CategoryId;
                advertisement.Price = updatedAd.Price ?? advertisement.Price;
                advertisement.Status = updatedAd.Status ?? advertisement.Status;

                if (updatedAd.Photos != null && updatedAd.Photos.Any())
                {
                    _db.Photos.RemoveRange(advertisement.Photos);

                    advertisement.Photos = new List<Photos>();
                    foreach (var photoBytes in updatedAd.Photos)
                    {
                        var photo = new Photos
                        {
                            AdvertisementId = advertisement.Id,
                            Image = photoBytes,
                            IsMain = false
                        };
                        advertisement.Photos.Add(photo);
                    }

                    if (advertisement.Photos.Count > 0)
                    {
                        advertisement.Photos.ElementAt(0).IsMain = true;
                    }
                }

                _db.Advertisements.Update(advertisement);
                await _db.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<GetAdvertisementDTO>(advertisement);
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Failed to update advertisement: " + ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<GetAdvertisementDTO>> GetAdvertisementById(int id)
        {
            var serviceResponse = new ServiceResponse<GetAdvertisementDTO>();
            try
            {
                var advertisement = await _db.Advertisements.FirstOrDefaultAsync(x => x.Id == id);
                var photos = await _db.Photos.Where(x => x.AdvertisementId == id).ToListAsync();

                if (advertisement != null)
                {
                    var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == advertisement.UserId);
                    var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == advertisement.CategoryId);

                    var advertisementDTO = _mapper.Map<GetAdvertisementDTO>(advertisement);
                    advertisementDTO.UserName = user != null ? user.UserName : "Unknown";
                    advertisementDTO.CategoryName = category != null ? category.CategoryName : "Unknown";

                    serviceResponse.Data = advertisementDTO;
                }
                else
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Advertisement not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Advertisement not found " + ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetAdvertisementsDTO>>> GetAdvertisements(string? name, int? categoryId, decimal? priceMin, decimal? priceMax, DateTime? postDate, int? status)
        {
            var serviceResponse = new ServiceResponse<List<GetAdvertisementsDTO>>();
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

                if (advertisements.Any())
                {
                    var advertisementsDTO = new List<GetAdvertisementsDTO>();

                    foreach (var advertisement in advertisements)
                    {
                        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == advertisement.UserId);
                        var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == advertisement.CategoryId);

                        var advertisementDTO = new GetAdvertisementsDTO
                        {
                            Id = advertisement.Id,
                            Name = advertisement.Name,
                            CategoryId = advertisement.CategoryId,
                            CategoryName = category != null ? category.CategoryName : "Unknown",
                            Price = advertisement.Price,
                            PostDate = advertisement.PostDate,
                            Status = advertisement.Status,
                            Photo = photos.FirstOrDefault(p => p.AdvertisementId == advertisement.Id),
                            UserId = advertisement.UserId,
                            UserName = user != null ? user.UserName : "Unknown"
                        };

                        advertisementsDTO.Add(advertisementDTO);
                    }

                    serviceResponse.Data = advertisementsDTO;
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
