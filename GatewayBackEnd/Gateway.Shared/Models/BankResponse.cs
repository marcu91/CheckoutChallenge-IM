using System;

namespace Gateway.Shared.Models
{
    public class BankResponse
    {
        public Guid BankResponseID { get; set; }
        public TransactionStatus Status { get;set; }
        public TransactionSubStatus SubStatus { get; set; }
    }
}