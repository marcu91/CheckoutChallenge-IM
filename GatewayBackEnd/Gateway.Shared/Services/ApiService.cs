using Gateway.Shared.Interfaces;
using Gateway.Shared.Models;
using Gateway.Shared.Representers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class ApiService : IApiService
    {
        private const string ProcessTransactionControllerRoute = "Transactions/transactions";
        private readonly IWebRequestService _webRequestService;

        public ApiService(IWebRequestService webRequestService)
        {
            this._webRequestService = webRequestService;
        }

        public async Task<HttpResponseMessage> ProcessTransactionAsync(TransactionRepresenter transaction, string bankURL)
        {
            if (transaction == null || bankURL == null) return null;

            var url = bankURL + ProcessTransactionControllerRoute;
            BankTransaction bankTransaction = GetBankTransaction(transaction);
            var contentString = JsonConvert.SerializeObject(bankTransaction);
            return await _webRequestService.MakeAsyncRequest(url, contentString).ConfigureAwait(false);
        }

        private BankTransaction GetBankTransaction(TransactionRepresenter transaction)
        {
            return new BankTransaction
            {
                TransactionAmount = transaction.Amount,
                CardNumber = transaction.Card.CardNumber,
                CardCvv = transaction.Card.Cvv,
                CardHolderName = transaction.Card.HolderName,
                CardExpiryMonth = transaction.Card.ExpiryMonth,
                CardExpiryYear = transaction.Card.ExpiryYear
            };
        }
    }
}
