namespace MarketBackEnd.PaymentsAndCart.DTOs.Orders
{
    public class AddOrderDTO
    {
        public int BuyerId { get; set; }
        public int SellerId { get; set; }
        public int AdvertisementId { get; set; }
        public int CategoryId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal Price { get; set; }
    }
}
