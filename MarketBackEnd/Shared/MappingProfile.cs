using AutoMapper;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Products.Advertisements.DTOs.Photo;
using MarketBackEnd.Products.Advertisements.Models;

namespace MarketBackEnd.Shared
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Advertisement, GetAdvertisementDTO>().ReverseMap();
            CreateMap<Advertisement, GetAdvertisementsDTO>().ReverseMap();
            CreateMap<Photos, GetPhotoDTO>().ReverseMap();
            CreateMap<CreateAdvertisementDTO, Advertisement>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
        }
    }
}
