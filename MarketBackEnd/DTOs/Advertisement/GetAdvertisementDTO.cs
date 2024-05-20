using MarketBackEnd.DTOs.Photo;
using MarketBackEnd.Model;

namespace MarketBackEnd.DTOs.Advertisement
{
    public class GetAdvertisementDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
        public List<Photos> Photos { get; set; }
    }
}
