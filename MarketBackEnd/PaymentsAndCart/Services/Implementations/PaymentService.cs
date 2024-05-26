﻿using AutoMapper;
using MarketBackEnd.PaymentsAndCart.DTOs.DebitCards;
using MarketBackEnd.PaymentsAndCart.Models;
using MarketBackEnd.PaymentsAndCart.Services.Interfaces;
using MarketBackEnd.Products.Advertisements.DTOs.Advertisement;
using MarketBackEnd.Products.Advertisements.Models;
using MarketBackEnd.Shared.Data;
using MarketBackEnd.Shared.Model;
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