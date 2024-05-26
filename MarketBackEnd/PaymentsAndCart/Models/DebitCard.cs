using MarketBackEnd.Shared.Model;
using System.ComponentModel.DataAnnotations;

namespace MarketBackEnd.PaymentsAndCart.Models
{
    public class DebitCard
    {
        public int Id { get; set; }
        public int UserId {  get; set; }
        public User User { get; set; }
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public decimal CardAmount { get; set; } = 100000;
    }
}
