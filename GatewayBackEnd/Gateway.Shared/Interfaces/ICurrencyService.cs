using Gateway.Data.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAllCurrenciesAsync();
        Task<Currency> GetCurrencyByIdAsync(int currencyID);
        Task<Currency> GetCurrencyByNameAsync(string currency);
    }
}