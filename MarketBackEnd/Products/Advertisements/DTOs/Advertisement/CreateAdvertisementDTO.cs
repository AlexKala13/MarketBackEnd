namespace MarketBackEnd.Products.Advertisements.DTOs.Advertisement
{
    public class CreateAdvertisementDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
        public List<byte[]> Photos { get; set; }
    }
}
