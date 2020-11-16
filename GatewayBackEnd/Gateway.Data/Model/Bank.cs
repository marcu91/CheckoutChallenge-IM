
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gateway.Data.Model
{
    public class Bank
    {
        [Key]
        public int BankID { get; set; }

        [Required(ErrorMessage = "Bank name is required")]
        [Column(TypeName = "NVARCHAR(200)")]
        public string BankName { get; set; }

        [Required(ErrorMessage = "Bank URL is required")]
        [Column(TypeName = "NVARCHAR(200)")]
        public string BankURL { get; set; }

        public List<Transaction> Transactions { get; set; }
    }
}