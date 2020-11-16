using Gateway.Data.Model;
using Gateway.Shared.Representers;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface IBankService
    {
        Task<BankResponseDto> ProcessTransactionAsync(TransactionRepresenter transactionRepresenter, string bankURL);
        Task<Bank> GetBankByNameAsync(string bank);
    }
}