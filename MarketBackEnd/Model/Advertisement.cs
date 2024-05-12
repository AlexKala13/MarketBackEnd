namespace MarketBackEnd.Model
{
    public class Advertisement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
    }
}
