using Assignment_04_EFC_.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04_EFC_.Models
{
    public class Account
    {
        [Key]
        [MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;

        public decimal CurrentBalance { get; set; }
        public AccountType AccountType { get; set; }
        public DateTime OpeningDate { get; set; }
        public AccountStatus AccountStatus { get; set; }

        [Required]
        [MaxLength(20)]
        public string BranchCode { get; set; } = string.Empty;

        public Branch Branch { get; set; } = null!;

        public ICollection<CustomerAccount> CustomerAccounts { get; set; } = new HashSet<CustomerAccount>();
        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

    }
}
