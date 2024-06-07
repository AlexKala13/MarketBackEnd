namespace MarketBackEnd.Users.Customer.DTOs
{
    public class GetUserInfoDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public decimal Balance { get; set; } = 0;
    }
}
