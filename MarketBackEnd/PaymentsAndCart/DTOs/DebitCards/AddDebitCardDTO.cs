namespace MarketBackEnd.PaymentsAndCart.DTOs.DebitCards
{
    public class AddDebitCardDTO
    {
        public int UserId { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
    }
}
