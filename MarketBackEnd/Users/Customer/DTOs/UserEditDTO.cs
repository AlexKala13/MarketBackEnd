namespace MarketBackEnd.Users.Customer.DTOs
{
    public class UserEditDTO
    {
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Telephone { get; set; }
        public string? Password { get; set; }
        public bool?  IsActive { get; set; }
    }
}
