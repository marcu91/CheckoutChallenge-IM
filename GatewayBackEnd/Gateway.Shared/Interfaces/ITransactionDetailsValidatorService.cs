using Gateway.Shared.Models;
using Gateway.Shared.Representers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface ITransactionDetailsValidatorService
    {
       Task<bool> ValidateAsync(TransactionRepresenter data);
        TransactionDetails GetTransactionDetails();
        List<string> GetErrors();
    }
}
