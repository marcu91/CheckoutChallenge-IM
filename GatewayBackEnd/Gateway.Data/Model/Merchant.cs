using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Data.Model
{
    public class Merchant
    {
        [Key]
        public Guid MerchantID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public List<Transaction> Transactions { get; set; }

    }
}