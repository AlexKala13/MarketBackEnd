using AutoMapper;
using MarketBackEnd.DTOs.Advertisement;
using MarketBackEnd.DTOs.Photo;
using MarketBackEnd.Model;

namespace MarketBackEnd
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Advertisement, GetAdvertisementDTO>().ReverseMap();
            CreateMap<Advertisement, GetAdvertisementsDTO>().ReverseMap();
            CreateMap<Photos, GetPhotoDTO>().ReverseMap();
        }
    }
}
