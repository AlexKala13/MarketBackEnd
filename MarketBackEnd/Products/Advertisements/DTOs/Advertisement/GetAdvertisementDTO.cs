﻿using MarketBackEnd.Products.Advertisements.Models;

namespace MarketBackEnd.Products.Advertisements.DTOs.Advertisement
{
    public class GetAdvertisementDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime DueDate { get; set; }
        public int Status { get; set; }
        public List<Photos> Photos { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
