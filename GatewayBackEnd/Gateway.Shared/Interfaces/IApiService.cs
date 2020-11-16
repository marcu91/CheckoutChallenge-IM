using Gateway.Shared.Representers;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Shared.Interfaces
{
    public interface IApiService
    {
        Task<HttpResponseMessage> ProcessTransactionAsync(TransactionRepresenter transaction, string bankURL);
    }
}
