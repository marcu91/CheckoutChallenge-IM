using Gateway.Shared.Representers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Gateway.API.Helpers
{
    /// <summary>
    /// A helper class ued to validate credit card information
    /// </summary>
    public static class CreditCardHelper
    {
        /// <summary>
        /// A method used to validate a card from a transaction request
        /// </summary>
        /// <param name="transactionDto">The transaction object</param>
        /// <returns>Whether the provided card inside the transaction object is valid</returns>
        public static bool IsCreditCardInfoValid(TransactionRepresenter transactionDto)
        {
            if (transactionDto == null) return false;
            var expiryConcatenated = $"{transactionDto.Card.ExpiryMonth}/{transactionDto.Card.ExpiryYear}";
            return IsCreditCardInfoValid(transactionDto.Card.CardNumber, expiryConcatenated, transactionDto.Card.Cvv);
        }

        /// <summary>
        /// A method used to validate card information 
        /// </summary>
        /// <param name="cardNumber">The credit card number</param>
        /// <param name="expiryDate">The expiry date string in MM/yyyy format</param>
        /// <param name="cvv">The CVV code, a 3 digit value</param>
        /// <returns>Whether the provided card data is valid</returns>
        public static bool IsCreditCardInfoValid(string cardNumber, string expiryDate, string cvv)
        {
            var cardCheck = new Regex(@"^(1298|1267|4512|4567|8901|8933|5170)([\-\s]?[0-9]{4}){3}$");
            var monthCheck = new Regex(@"^(0[1-9]|1[0-2])$");
            var yearCheck = new Regex(@"^20[0-9]{2}$");
            var cvvCheck = new Regex(@"^\d{3}$");

            if (!cardCheck.IsMatch(cardNumber))
                return false;

            if (!cvvCheck.IsMatch(cvv))
                return false;

            if (expiryDate == null)
                return false;

            var dateParts = expiryDate.Split('/');      
            if (!monthCheck.IsMatch(dateParts[0]))
                return false;

            if (!yearCheck.IsMatch(dateParts[1]))
                return false;

            var year = int.Parse(dateParts[1], CultureInfo.InvariantCulture);
            var month = int.Parse(dateParts[0], CultureInfo.InvariantCulture);
            var lastDateOfExpiryMonth = DateTime.DaysInMonth(year, month);
            var cardExpiry = new DateTime(year, month, lastDateOfExpiryMonth, 23, 59, 59);

            return (cardExpiry > DateTime.Now && cardExpiry < DateTime.Now.AddYears(6));
        }

        /// <summary>
        /// A method that will mask the card number, revealing only the last 4 digits
        /// </summary>
        /// <param name="cardNumber">The original card number</param>
        /// <returns>A masked card number with only the last 4 digits visible</returns>
        public static string MaskCardNumber(string cardNumber)
        {
            if (cardNumber == null) return cardNumber;
            var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);
            var requiredMask = new String('X', cardNumber.Length - lastDigits.Length);
            var maskedString = string.Concat(requiredMask, lastDigits);
            return maskedString;
        }
    }
}