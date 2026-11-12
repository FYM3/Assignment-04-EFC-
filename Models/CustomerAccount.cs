using Assignment_04_EFC_.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04_EFC_.Models
{
    public class CustomerAccount
    {
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        [MaxLength(20)]
        public string AccountNumber { get; set; } = string.Empty;
        public Account Account { get; set; } = null!;

        public OwnershipType OwnershipType { get; set; }
        public DateTime OwnershipStartDate { get; set; }

    }
}
