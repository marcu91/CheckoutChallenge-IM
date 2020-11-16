using Gateway.Data.Model;
using Gateway.Interfaces.Services;
using Gateway.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IRepositoryService _contextService;
        public TransactionService(IRepositoryService contextService)
        {
            _contextService = contextService;
        }

        public async Task<Transaction> CreateTransactionAsync(Transaction entity)
        {
            return await _contextService
                 .AddAsync(entity)
                 .ConfigureAwait(false);
        }

        public async Task<Transaction> GetTransactionById(Guid transactionID)
        {
            return await _contextService
                .Find<Transaction>(t => t.TransactionID == transactionID)
                .Include(x => x.Bank)
                .Include(x => x.Currency)
                .Include(x => x.Card)
                .Include(x => x.Merchant)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<List<Transaction>> GetTransactionsByMerchantID(Guid merchantID)
        {
            return await _contextService
                 .Find<Transaction>(t => t.MerchantID == merchantID)
                 .Include(x => x.Bank)
                 .Include(x => x.Currency)
                 .Include(x => x.Card)
                 .Include(x => x.Merchant)
                 .ToListAsync()
                 .ConfigureAwait(false);
        }

        public async Task<bool> UpdateTransaction(Transaction transaction)
        {
            return await _contextService
                 .UpdateAsync(transaction)
                 .ConfigureAwait(false);
        }
    }
}