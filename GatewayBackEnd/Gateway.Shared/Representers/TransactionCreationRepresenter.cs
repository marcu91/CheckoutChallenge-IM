using System;

namespace Gateway.Shared.Representers
{
    public class TransactionCreationRepresenter
    {
        public Guid BankResponseID { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public Guid TransactionID { get; set; }
    }
}