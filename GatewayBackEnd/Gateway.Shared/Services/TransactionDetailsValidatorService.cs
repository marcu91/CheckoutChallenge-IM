using Gateway.Shared.Interfaces;
using Gateway.Shared.Models;
using Gateway.Shared.Representers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.Shared.Services
{
    public class TransactionDetailsValidatorService : ITransactionDetailsValidatorService
    {
        private TransactionRepresenter TransactionRepresenter { get; set; }

        private readonly IMerchantService _merchantService;
        private readonly ICurrencyService _currencyService;
        private readonly IBankService _bankService;

        public TransactionDetails TransactionDetails { get; }
        public List<string> Errors { get; }

        public TransactionDetailsValidatorService(
            IMerchantService merchantService,
            ICurrencyService currencyService,
            IBankService bankService
            )
        {
            this.TransactionDetails = new TransactionDetails();
            this.Errors = new List<string>();
            this._merchantService = merchantService;
            this._currencyService = currencyService;
            this._bankService = bankService;
        }

        public async Task<bool> ValidateAsync(TransactionRepresenter data)
        {
            this.SetTransactionData(data);
            this.CheckAmount();
            await this.CheckMerchantData().ConfigureAwait(false);
            await this.CheckCurrencyData().ConfigureAwait(false);
            await this.CheckBankData().ConfigureAwait(false);
            return this.Errors.Any();
        }

        public TransactionDetails GetTransactionDetails()
        {
            return this.TransactionDetails;
        }
        public List<string> GetErrors()
        {
            return this.Errors;
        }

        private void SetTransactionData(TransactionRepresenter data)
        {
            this.TransactionRepresenter = data;
        }

        private void CheckAmount()
        {
            if (TransactionRepresenter.Amount <= 0)
               Errors.Add("Amount is invalid");
        }

        private async Task CheckMerchantData()
        {
            if (!Guid.TryParse(TransactionRepresenter.MerchantID, out _))
                Errors.Add("Merchant is invalid");

            var merchant = await _merchantService
                                .GetMerchantAsync(Guid.Parse(TransactionRepresenter.MerchantID))
                                .ConfigureAwait(false);

            if (merchant == null) Errors.Add("Merchant is invalid");
            this.TransactionDetails.Merchant = merchant;
            
        }
         
        private async Task CheckCurrencyData()
        {
            var currency = await _currencyService
                                 .GetCurrencyByNameAsync(TransactionRepresenter.Currency)
                                 .ConfigureAwait(false);
            
            if (currency == null) Errors.Add("Currency not supported");
            this.TransactionDetails.Currency = currency;
        }

        private async Task CheckBankData()
        {
            var bank = await _bankService
                            .GetBankByNameAsync(TransactionRepresenter.Bank)
                            .ConfigureAwait(false);

            if (bank == null) Errors.Add("Bank not supported");
            this.TransactionDetails.Bank = bank;
        }
    }
}
