namespace MarketBackEnd.PaymentsAndCart.DTOs.DebitCards
{
    public class GetDebitCardDTO
    {
        public int Id { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public decimal CardAmount { get; set; }
    }
}
