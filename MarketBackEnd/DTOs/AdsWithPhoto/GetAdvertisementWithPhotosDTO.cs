using MarketBackEnd.DTOs.Advertisement;
using MarketBackEnd.DTOs.Photo;

namespace MarketBackEnd.DTOs.AdsWithPhoto
{
    public class GetAdvertisementWithPhotosDTO
    {
        public GetAdvertisementDTO AdverisementInfo { get; set; }

        public List<GetPhotoDTO>? Photos { get; set; }
    }
}
