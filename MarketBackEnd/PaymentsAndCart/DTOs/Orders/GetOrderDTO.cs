using MarketBackEnd.Products.Advertisements.Models;
using MarketBackEnd.Shared.Model;

namespace MarketBackEnd.PaymentsAndCart.DTOs.Orders
{
    public class GetOrderDTO
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public string BuyerName { get; set; }
        public int SellerId { get; set; }
        public string SellerName { get; set; }
        public int AdvertisementId { get; set; }
        public int Status { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
