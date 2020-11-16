using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Gateway.API.Helpers;
using Gateway.API.Representers;
using Gateway.Data.Model;
using Gateway.Shared.Interfaces;
using Gateway.Shared.Models;
using Gateway.Shared.Representers;
using Gateway.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Gateway.API.Controllers
{
    /// <summary>
    /// A controller used to process transactions and retrieve transactions made by merchants
    /// </summary>
    [Authorize(Roles = "GatewayAdministrator, GatewayMerchantUser")]
    [ApiController]
    [Route("[controller]")] 
    public class TransactionsController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ICardDetailsService _cardDetailsService;
        private readonly IMerchantService _merchantService;
        private readonly ITransactionService _transactionService;
        private readonly IBankService _bankService;
        private readonly ITransactionDetailsValidatorService _transactionDetailsValidatorService;

        /// <summary>
        /// A constructor used to provide an instance of the TransactionsController
        /// </summary>
        /// <param name="currencyService">A service that retrieves the available gateway currencies</param>
        /// <param name="cardDetailsService">A service used for card details processing</param>
        /// <param name="merchantService">A service that retrieves the available gateway merchants</param>
        /// <param name="transactionService">A service used to create or retrieve transactions</param>
        /// <param name="bankService">A service that retrieves the available gateway banks</param>
        /// <param name="transactionDetailsValidatorService">A service used to validate the transaction content</param>
        public TransactionsController(
            ICurrencyService currencyService,
            ICardDetailsService cardDetailsService,
            IMerchantService merchantService, 
            ITransactionService transactionService,
            IBankService bankService,
            ITransactionDetailsValidatorService transactionDetailsValidatorService)
        {
            _currencyService = currencyService;
            _cardDetailsService = cardDetailsService;
            _merchantService = merchantService;
            _transactionService = transactionService;
            _bankService = bankService;
            _transactionDetailsValidatorService = transactionDetailsValidatorService;
        }

        /// <summary>
        /// A controller action used to process an incoming transaction request
        /// </summary>
        /// <param name="transaction">The transaction request payload inside the request body</param>
        /// <returns>A response containing the newly processed transaction or a bad request with details</returns>
        [HttpPost]
        [Route("process")]
        public async Task<IActionResult> ProcessTransaction([FromBody]TransactionRepresenter transaction)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(transaction == null)
                return BadRequest("Transaction data not found");

            if (await _transactionDetailsValidatorService.ValidateAsync(transaction).ConfigureAwait(false))
                return BadRequest(_transactionDetailsValidatorService.GetErrors());

            var creditCardValidationResult = CreditCardHelper.IsCreditCardInfoValid(transaction);
            if (!creditCardValidationResult)
            {
                return BadRequest("Invalid Credit Card data");
            }

            var transactionDetails = _transactionDetailsValidatorService.GetTransactionDetails();
            var cardEntity = await _cardDetailsService.CreateCardDetailsAsync(transaction).ConfigureAwait(false);
           

            BankResponseDto bankResponse = await _bankService.ProcessTransactionAsync(transaction, transactionDetails.Bank.BankURL).ConfigureAwait(false);
            Transaction transactionEntity = GetTransactionEntity(bankResponse, transaction, transactionDetails, cardEntity);
           
            await _transactionService.CreateTransactionAsync(transactionEntity).ConfigureAwait(false);

            return Ok(transactionEntity);
        }

        /// <summary>
        /// A controller action used to get all actions from a merchant based on its unique identifier
        /// </summary>
        /// <param name="merchantID">The merchant unique identifier</param>
        /// <returns>A list of transactions</returns>
        [HttpGet]
        [Route("GetTransactionsByMerchantID/{merchantId}")]
        public async Task<IActionResult> GetTransactionsByMerchantID(Guid merchantID)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var transactions = await _transactionService.GetTransactionsByMerchantID(merchantID).ConfigureAwait(false);

            transactions.ForEach(x => x.Card.CardNumber = CreditCardHelper.MaskCardNumber(x.Card.CardNumber));

            if (!transactions.Any()) return NotFound();
            return Ok(transactions);
        }

        /// <summary>
        /// Get transactionb by ID for reconcilliation purposes
        /// </summary>
        /// <param name="transactionID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetTransactionById")]
        public async Task<IActionResult> GetTransactionById(Guid transactionID)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = new TransactionResponseRepresenter();
            var entity = await _transactionService.GetTransactionById(transactionID).ConfigureAwait(false);
            var currency = await _currencyService.GetCurrencyByIdAsync(entity.CurrencyId).ConfigureAwait(false);
            var card = await _cardDetailsService.GetCardDetailsByIdAsync(entity.CardDetailsID).ConfigureAwait(false);

            card.CardNumber = CreditCardHelper.MaskCardNumber(card.CardNumber);
            response.Currency = currency.Name;
            response.Amount = entity.Amount;
            response.BankReferenceID = entity.BankReferenceID;
            response.Status = entity.Status;
            response.SubStatus = entity.SubStatus;
            response.Card = card;

            return Ok(response);
        }

        private static Transaction GetTransactionEntity(
            BankResponseDto bankResponse,
            TransactionRepresenter transactionRepresenter,
            TransactionDetails transactionDetails,
            CardDetails cardEntity)
        {
            return new Transaction
            {
                Amount = transactionRepresenter.Amount,
                Card = cardEntity,
                CardDetailsID = cardEntity.CardDetailsID,
                Currency = transactionDetails.Currency,
                CurrencyId = transactionDetails.Currency.CurrencyId,
                Merchant =transactionDetails.Merchant,
                MerchantID = transactionDetails.Merchant.MerchantID,
                TransactionID = Guid.NewGuid(),
                Bank = transactionDetails.Bank,
                BankID = transactionDetails.Bank.BankID,
                BankReferenceID = bankResponse.BankResponseID,
                Status = bankResponse.Status,
                SubStatus = bankResponse.SubStatus
            };
        }
    }
}