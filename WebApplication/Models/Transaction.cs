using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Display(Name = "Concepto")]
        public string Concept { get; set; }

        [Display(Name = "Value")]
        public decimal Value { get; set; }

        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; }
    }
}
