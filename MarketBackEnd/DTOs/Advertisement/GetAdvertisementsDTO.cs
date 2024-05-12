namespace MarketBackEnd.DTOs.Advertisement
{
    public class GetAdvertisementsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public int Status { get; set; }
    }
}
