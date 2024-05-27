using MarketBackEnd.PaymentsAndCart.DTOs.DebitCards;
using MarketBackEnd.PaymentsAndCart.DTOs.Orders;
using MarketBackEnd.Shared.Model;

namespace MarketBackEnd.PaymentsAndCart.Services.Interfaces
{
    public interface ICardService
    {
        Task<ServiceResponse<List<GetDebitCardDTO>>> GetDebitCards(int userId);
        Task<ServiceResponse<GetDebitCardDTO>> AddDebitCard(AddDebitCardDTO newCard);
        Task<ServiceResponse<string>> DeleteDebitCard(int id, int userId);
        Task<ServiceResponse<bool>> AddToBalance(int debitCardId, int userId, decimal amount);
        Task<ServiceResponse<bool>> AddToCard(int debitCardId, int userId, decimal amount);
    }
}
