using Gateway.Data.Model;
using Gateway.Shared.Representers;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface ICardDetailsService
    {
        Task<CardDetails> GetCardDetailsByNumberAsync(string cardNumber);
        Task<CardDetails> GetCardDetailsByIdAsync(int cardID);
        Task AddCardAsync(CardDetails card);
        Task<CardDetails> CreateCardDetailsAsync(TransactionRepresenter transactionRepresenter);
    }
}