namespace MarketBackEnd.Shared.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public DateTime RegisterDate { get; set; } = DateTime.Now;
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsAdmin { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public decimal Balance { get; set; } = 0;
    }
}
