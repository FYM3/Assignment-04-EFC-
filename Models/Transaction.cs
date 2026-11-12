using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04_EFC_.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required, MaxLength(30)]
        public string TransactionNumber { get; set; } = string.Empty;

        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }

        [Required, MaxLength(50)]
        public string TransactionType { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Note { get; set; }

        [Required, MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;
        public Account Account { get; set; } = null!;
    }
}
