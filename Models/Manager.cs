using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_04_EFC_.Models
{
    public class Manager
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime HireDate { get; set; }

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        public Branch? Branch { get; set; }

    }
}
