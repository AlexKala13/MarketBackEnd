﻿using MarketBackEnd.Products.Advertisements.Models;
using Microsoft.EntityFrameworkCore;
using MarketBackEnd.Shared.Model;
using MarketBackEnd.PaymentsAndCart.Models;

namespace MarketBackEnd.Shared.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Advertisement> Advertisements => Set<Advertisement>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Photos> Photos => Set<Photos>();
        public DbSet<DebitCard> DebitCards => Set<DebitCard>();
        public DbSet<Orders> Orders => Set<Orders>();
    }
}
