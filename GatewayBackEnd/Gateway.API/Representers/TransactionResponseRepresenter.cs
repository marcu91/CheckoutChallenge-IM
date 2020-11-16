using Gateway.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gateway.API.Representers
{
    public class TransactionResponseRepresenter
    {
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public Guid BankReferenceID { get; set; }
        public CardDetails Card { get; set; }
    }
}