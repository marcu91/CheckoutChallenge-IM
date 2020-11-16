using Gateway.Data.Model;
using Gateway.Interfaces.Services;
using Gateway.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class MerchantService : IMerchantService
    {
        private readonly IRepositoryService _contextService;

        public MerchantService(IRepositoryService contextService)
        {
            _contextService = contextService;
        }

        public async Task<Merchant> GetMerchantAsync(Guid merchantId)
        {
            return await _contextService
                .Find<Merchant>(m => m.MerchantID == merchantId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }
    }
}