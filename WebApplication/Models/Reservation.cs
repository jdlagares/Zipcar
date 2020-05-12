using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Enumerations;

namespace WebApplication.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        public int CarId { get; set; }

        public Car Car { get; set; }

        public EnumPlan Plan { get; set; }

        public DateTime DateStart { get; set; }

        public DateTime EstimateDateEnd { get; set; }

        public DateTime? RealDateEnd { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        public bool Returned { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

    }
}
