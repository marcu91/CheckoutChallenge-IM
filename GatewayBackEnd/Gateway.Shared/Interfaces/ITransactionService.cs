using Gateway.Data.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> CreateTransactionAsync(Transaction entity);
        Task<Transaction> GetTransactionById(Guid transactionID);
        Task<List<Transaction>> GetTransactionsByMerchantID(Guid merchantID);
        Task<bool> UpdateTransaction(Transaction transaction);
    }
}