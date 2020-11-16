using Gateway.MockBank.Moodels;
using Gateway.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Gateway.MockBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionsController : ControllerBase
    {
        /// <summary>
        /// Process a Transaction
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("transactions", Name = "ProcessTransaction")]
        public IActionResult ProcessTransaction([FromBody]MockTransaction transaction)
        {
            var response = new BankResponse();

            //We are interested in what responses the mocked bank is giving us, not how, in this case
            switch (transaction.TransactionAmount)
            {
                case 200:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Successful;
                    response.SubStatus = TransactionSubStatus.Successful;
                    break;
                case 05:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.DeclinedDonothonour;
                    break;
                case 12:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.InvalidPayment;
                    break;
                case 14:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.InvalidCardNumber;
                    break;
                case 51:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.InsufficientFunds;
                    break;
                case 54:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.BadTrackData;
                    break;
                case 62:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.RestrictedCard;
                    break;
                case 63:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.SecurityViolation;
                    break;
                case 9998:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.ResponseTimeout;
                    break;
                case 150:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.CardNot3DSecureEnabled;
                    break;
                case 6900:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.UnableToSpecifyIfCardIsSecureEnabled;
                    break;
                case 5000:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.SecureSystemMalfunction3DSecure;
                    break;
                case 6510:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.SecureAuthenticationRequired3DSecure;
                    break;
                case 33:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.ExpiredCard;
                    break;
                case 4001:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.PaymentBlockedDueToRisk;
                    break;
                case 4008:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.CardNumberBlacklisted;
                    break;
                case 2011:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.StopPaymenttThisAuth;
                    break;
                case 2013:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Failed;
                    response.SubStatus = TransactionSubStatus.StopPaymentAll;
                    break;
                default:
                    response.BankResponseID = Guid.NewGuid();
                    response.Status = TransactionStatus.Successful;
                    response.SubStatus = TransactionSubStatus.Successful;
                    break;
            }

            return Ok(response);
        }
    }
}