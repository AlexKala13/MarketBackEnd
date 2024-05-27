using AutoMapper;
using MarketBackEnd.PaymentsAndCart.DTOs.DebitCards;
using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.PaymentsAndCart.Models;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Products.Advertisements.Models;
using MarketBackEnd.Shared.Data;
using MarketBackEnd.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketBackEnd.PaymentsAndCart.Services.Implementations
{
    public class CardService : ICardService
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CardService(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetDebitCardDTO>> AddDebitCard(AddDebitCardDTO newCard)
        {
            var response = new ServiceResponse<GetDebitCardDTO>();
            try
            {
                if (newCard.CardNumber == null || newCard.CardName == null)
                {
                    response.Success = false;
                    response.Message = "Debit card parameters are empty or null.";
                    return response;
                }
                var debitCard = _mapper.Map<DebitCard>(newCard);

                await _db.DebitCards.AddAsync(debitCard);
                await _db.SaveChangesAsync();

                response.Data = _mapper.Map<GetDebitCardDTO>(debitCard);
                response.Success = true;
                response.Message = "Debit card added successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> AddToBalance(int debitCardId, int userId, decimal amount)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var debitCard = await _db.DebitCards.FirstOrDefaultAsync(x => x.Id == debitCardId && x.UserId == userId);
                if (debitCard == null)
                {
                    response.Success = false;
                    response.Message = "Debit card not found.";
                    return response;
                }

                if (debitCard.CardAmount < amount)
                {
                    response.Success = false;
                    response.Message = "Insufficient funds on the debit card.";
                    return response;
                }

                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    return response;
                }

                debitCard.CardAmount -= amount;
                user.Balance += amount;

                _db.DebitCards.Update(debitCard);
                _db.Users.Update(user);

                await _db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Balance updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<bool>> AddToCard(int debitCardId, int userId, decimal amount)
        {
            var response = new ServiceResponse<bool>();
            try
            {
                var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    response.Success = false;
                    response.Message = "User not found.";
                    return response;
                }

                if (user.Balance < amount)
                {
                    response.Success = false;
                    response.Message = "Insufficient funds in the user balance.";
                    return response;
                }

                var debitCard = await _db.DebitCards.FirstOrDefaultAsync(x => x.Id == debitCardId && x.UserId == userId);
                if (debitCard == null)
                {
                    response.Success = false;
                    response.Message = "Debit card not found.";
                    return response;
                }

                user.Balance -= amount;
                debitCard.CardAmount += amount;

                _db.Users.Update(user);
                _db.DebitCards.Update(debitCard);

                await _db.SaveChangesAsync();

                response.Success = true;
                response.Message = "Card balance updated successfully.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<string>> DeleteDebitCard(int id, int userId)
        {
            var response = new ServiceResponse<string>();

            try
            {
                var debitCard = await _db.DebitCards.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
                if (debitCard == null)
                {
                    response.Success = false;
                    response.Message = "Debit card not found.";
                    return response;
                }

                _db.DebitCards.Remove(debitCard);
                await _db.SaveChangesAsync();

                response.Data = "Debit card deleted successfully.";
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }

        public async Task<ServiceResponse<List<GetDebitCardDTO>>> GetDebitCards(int userId)
        {
            var response = new ServiceResponse<List<GetDebitCardDTO>>();
            try
            {
                var debitCards = await _db.DebitCards.Where(x => x.UserId == userId).ToListAsync();
                var debitCardDTOs = _mapper.Map<List<GetDebitCardDTO>>(debitCards);

                response.Data = debitCardDTOs;
                response.Success = true;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"An error occurred: {ex.Message}";
            }

            return response;
        }
    }
}
