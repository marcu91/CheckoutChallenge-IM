using System;

namespace Gateway.Shared.Models
{
    [Serializable]
    public class BankTransaction
    {
        public decimal TransactionAmount { get; set; }
        public string CardNumber { get; set; }
        public string CardCvv { get; set; }
        public string CardHolderName { get; set; }
        public string CardExpiryMonth { get; set; }
        public int CardExpiryYear { get; set; }
    }
}
