using Gateway.Data.Model;

namespace Gateway.Shared.Models
{
    public class TransactionDetails
    {
        public TransactionDetails()
        {
           
        }

        public Merchant Merchant { get; set; }
        public Currency Currency { get; set; }
        public Bank Bank { get; set; }
    }
}
