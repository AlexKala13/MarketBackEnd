namespace MarketBackEnd.Products.Advertisements.Models
{
    public class Photos
    {
        public int Id { get; set; }
        //public Advertisement Advertisement { get; set; }
        public int AdvertisementId { get; set; }
        public byte[] Image { get; set; }
        public bool IsMain { get; set; } = false;
    }
}
