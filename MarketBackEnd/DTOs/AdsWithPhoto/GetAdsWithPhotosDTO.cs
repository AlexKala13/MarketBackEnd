using MarketBackEnd.DTOs.Advertisement;
using MarketBackEnd.DTOs.Photo;

namespace MarketBackEnd.DTOs.AdsWithPhoto
{
    public class GetAdsWithPhotosDTO
    {
        public List<GetAdvertisementsDTO> Advertisements { get; set; }
        public List<GetPhotoDTO>? Photos { get; set; }
    }
}
