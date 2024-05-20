namespace MarketBackEnd.Products.Advertisements.DTOs.Photo
{
    public class GetPhotoDTO
    {
        public int Id { get; set; }
        public int AdvertisementId { get; set; }
        public byte[] Image { get; set; }
        public bool IsMain { get; set; }
    }
}
