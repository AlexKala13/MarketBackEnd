using AutoMapper;
using MarketBackEnd.PaymentsAndCart.DTOs.DebitCards;
using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.PaymentsAndCart.Models;
using MarketBackEnd.Products.Advertisements.DTOs;
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
            CreateMap<NewAdvertisementDTO, Advertisement>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
            //.ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos.Select(photo => new Photos { Image = photo })));
            CreateMap<Photos, GetPhotoDTO>().ReverseMap();
            CreateMap<CreateAdvertisementDTO, Advertisement>()
                .ForMember(dest => dest.Photos, opt => opt.Ignore());
            CreateMap<DebitCard, GetDebitCardDTO>().ReverseMap();
            CreateMap<DebitCard, AddDebitCardDTO>().ReverseMap();
            CreateMap<Orders, GetOrderDTO>().ReverseMap();
            CreateMap<Orders, AddOrderDTO>().ReverseMap();
        }
    }
}
