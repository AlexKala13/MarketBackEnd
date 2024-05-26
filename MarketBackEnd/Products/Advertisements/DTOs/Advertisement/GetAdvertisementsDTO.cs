using MarketBackEnd.Products.Advertisements.Models;

namespace MarketBackEnd.Products.Advertisements.DTOs.Advertisement
{
    public class GetAdvertisementsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public int Status { get; set; }
        public Photos? Photo { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
