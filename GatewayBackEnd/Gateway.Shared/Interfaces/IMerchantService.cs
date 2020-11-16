using Gateway.Data.Model;
using System;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface IMerchantService
    {
        Task<Merchant> GetMerchantAsync(Guid merchantId);
    }
}