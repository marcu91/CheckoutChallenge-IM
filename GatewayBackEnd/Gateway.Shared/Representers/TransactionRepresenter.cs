using Gateway.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace Gateway.Shared.Representers
{
    [Serializable]
    public class TransactionRepresenter
    {
        [Required(ErrorMessage = "Currency is required")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [DataType("decimal(18,5)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Card details are required")]
        public CardDetails Card { get; set; }

        [Required(ErrorMessage = "MerchantID is required")]
        public string MerchantID { get; set; }

        [Required(ErrorMessage = "Bank is required")]
        public string Bank { get; set; }
    }
}