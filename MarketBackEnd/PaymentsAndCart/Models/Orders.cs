using MarketBackEnd.Products.Advertisements.Models;
using MarketBackEnd.Shared.Model;

namespace MarketBackEnd.PaymentsAndCart.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public User Buyer { get; set; }
        public int SellerId { get; set; }
        public User Seller { get; set; }
        public int AdvertisementId { get; set; }
        public Advertisement Advertisement { get; set; }
        public int Status { get; set; } = 0;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    }
}
