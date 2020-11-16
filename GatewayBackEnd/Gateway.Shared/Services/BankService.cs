using Gateway.Data.Model;
using Gateway.Interfaces.Services;
using Gateway.Shared.Interfaces;
using Gateway.Shared.Models;
using Gateway.Shared.Representers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class BankService : IBankService
    {
        private readonly IRepositoryService _contextService;
        private readonly IApiService _apiService;


        public BankService(IRepositoryService contextService, IApiService apiService)
        {
            _contextService = contextService ?? throw new ArgumentNullException();
            _apiService = apiService ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Get Bank by bank name
        /// </summary>
        /// <param name="bankName">The bank name</param>
        /// <returns>An instace of the Bank </returns>
        public async Task<Bank> GetBankByNameAsync(string bankName)
        {
            return await _contextService
                .Find<Bank>(b => b.BankName == bankName)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Process a tranction through an acquirer
        /// </summary>
        /// <param name="transactionRepresenter"></param>
        /// <param name="bankURL"></param>
        /// <returns></returns>
        public async Task<BankResponseDto> ProcessTransactionAsync(TransactionRepresenter transactionRepresenter, string bankURL)
        {
            if (transactionRepresenter == null || bankURL == null) return null;

            //Process transaction through acquirer
            var bankResponse = await _apiService.ProcessTransactionAsync(transactionRepresenter, bankURL)
                                                .ConfigureAwait(false);

            var bankResponseData = await bankResponse.Content
                                                     .ReadAsStringAsync()
                                                     .ConfigureAwait(false);


            var json = JsonConvert.DeserializeObject<BankResponse>(bankResponseData);
            var transactionCreationRepresenter = new BankResponseDto
            {
                BankResponseID = json.BankResponseID,
                Status = json.Status.ToString(),
                SubStatus = json.SubStatus.ToString(),
            };

            return transactionCreationRepresenter;
        }
    }
}