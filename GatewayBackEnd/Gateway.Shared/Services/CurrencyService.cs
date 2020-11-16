using Gateway.Data.Model;
using Gateway.Interfaces.Services;
using Gateway.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly IRepositoryService _contextService;
        public CurrencyService(IRepositoryService contextService)
        {
            _contextService = contextService;
        }

        public async Task<List<Currency>> GetAllCurrenciesAsync()
        {
            return await _contextService
                .Find<Currency>(x => x.CurrencyId != 0)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task<Currency> GetCurrencyByNameAsync(string currency)
        {
            return await _contextService
                .Find<Currency>(c => c.Name == currency)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        public async Task<Currency> GetCurrencyByIdAsync(int currencyID)
        {
            return await _contextService
                .Find<Currency>(c => c.CurrencyId == currencyID)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }
    }
}