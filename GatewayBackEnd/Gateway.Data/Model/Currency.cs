using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Data.Model
{
    public class Currency
    {
        [Key]
        public int CurrencyId { get; set; }
        [Required(ErrorMessage = "Currency name is required")]
        public string Name { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}