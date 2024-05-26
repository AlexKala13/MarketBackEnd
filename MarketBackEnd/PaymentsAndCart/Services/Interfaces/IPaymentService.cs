using MarketBackEnd.PaymentsAndCart.DTOs.DebitCards;
using MarketBackEnd.Shared.Model;

namespace MarketBackEnd.PaymentsAndCart.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<ServiceResponse<List<GetDebitCardDTO>>> GetDebitCards(int userId);
        Task<ServiceResponse<GetDebitCardDTO>> AddDebitCard(AddDebitCardDTO newCard);
        Task<ServiceResponse<string>> DeleteDebitCard(int id, int userId);
    }
}
