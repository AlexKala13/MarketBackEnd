using AutoMapper;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
using MarketBackEnd.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.PaymentsAndCart.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PaymentService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> AdvertisementPayment(int userId, int? status)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var admin = await _db.Users.FirstOrDefaultAsync(u => u.IsAdmin == true);

            if (user == null)
            {
                return false;
            }

            decimal amountToDeduct = status switch
            {
                0 => 0.1m,
                1 => 0.5m,
                2 => 1.0m,
                _ => 0.1m
            };

            if (user.Balance < amountToDeduct)
            {
                return false;
            }

            user.Balance -= amountToDeduct;
            admin.Balance += amountToDeduct;

            _db.Users.Update(user);
            _db.Users.Update(admin);

            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ProductPurchase(int? debitCardId, int buyerId, int sellerId, decimal price)
        {
            var seller = await _db.Users.FirstOrDefaultAsync(s => s.Id == sellerId);
            if (debitCardId != null)
            {
                var debitCard = await _db.DebitCards.FirstOrDefaultAsync(c => c.Id == debitCardId && c.UserId == buyerId);
                if (debitCard == null)
                {
                    return false;
                }
                if (debitCard.CardAmount < price)
                {
                    return false;
                }

                debitCard.CardAmount -= price;
                seller.Balance += price;

                _db.DebitCards.Update(debitCard);
                _db.Users.Update(seller);
            }
            else
            {
                var buyer = await _db.Users.FirstOrDefaultAsync(u => u.Id == buyerId);
                if (buyer == null)
                {
                    return false;
                }
                if (buyer.Balance < price)
                {
                    return false;
                }

                buyer.Balance -= price;
                seller.Balance += price;

                _db.Users.Update(buyer);
                _db.Users.Update(seller);
            }

            await _db.SaveChangesAsync();
            return true;
        }
    }
}
