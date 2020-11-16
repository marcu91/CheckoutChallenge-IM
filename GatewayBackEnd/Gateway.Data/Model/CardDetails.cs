using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Data.Model
{
    public class CardDetails
    {
        [Key]
        [JsonIgnore]
        public int CardDetailsID { get; set; }

        [Required(ErrorMessage = "Card number is required")]
        [CreditCard]
        [Column(TypeName = "VARCHAR(25)")]
        public string CardNumber { get; set; } 

        [Required(ErrorMessage = "Cvv is required")]
        [MaxLength(4)]
        [Column(TypeName = "VARCHAR(4)")]
        public string Cvv { get; set; }

        [Required(ErrorMessage = "Holder Name is required")]
        [Column(TypeName = "NVARCHAR(200)")]
        public string HolderName { get; set; }

        [Required(ErrorMessage = "Expiry Month is required")]
        [Column(TypeName = "VARCHAR(2)")]
        public string ExpiryMonth { get; set; }

        [Required(ErrorMessage = "Expiry year is required")]
        [Column(TypeName = "SMALLINT")]
        public int ExpiryYear { get; set; }

        public List<Transaction> Transactions { get; set; }

    }
}