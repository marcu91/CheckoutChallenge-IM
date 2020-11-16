using Gateway.Data.Model;
using Gateway.Interfaces.Services;
using Gateway.Shared.Interfaces;
using Gateway.Shared.Representers;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class CardDetailsService : ICardDetailsService
    {
        private readonly IRepositoryService _contextService;

        public CardDetailsService(IRepositoryService contextService)
        {
            _contextService = contextService;
        }

        public async Task<CardDetails> CreateCardDetailsAsync(TransactionRepresenter transactionRepresenter)
        {
            if (transactionRepresenter == null || transactionRepresenter.Card == null) return null;

            var cardEntity = await this.GetCardDetailsByNumberAsync(transactionRepresenter.Card.CardNumber).ConfigureAwait(false);
            if (cardEntity == null)
            {
                cardEntity = new CardDetails()
                {
                    CardNumber = transactionRepresenter.Card.CardNumber,
                    Cvv = transactionRepresenter.Card.Cvv,
                    ExpiryMonth = transactionRepresenter.Card.ExpiryMonth,
                    ExpiryYear = transactionRepresenter.Card.ExpiryYear,
                    HolderName = transactionRepresenter.Card.HolderName,
                };

                await this.AddCardAsync(cardEntity).ConfigureAwait(false);
            };
            return cardEntity;
        }

        public async Task<CardDetails> GetCardDetailsByNumberAsync(string cardNumber)
        {
            return await _contextService
                .Find<CardDetails>(cd => cd.CardNumber == cardNumber)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<CardDetails> GetCardDetailsByIdAsync(int cardID)
        {
            return await _contextService
                .Find<CardDetails>(cd => cd.CardDetailsID == cardID)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task AddCardAsync(CardDetails card)
        {
            await _contextService.AddAsync(card).ConfigureAwait(false);
        }
    }
}