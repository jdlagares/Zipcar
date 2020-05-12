using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Concept { get; set; }

        public decimal Value { get; set; }

        public int ReservationId { get; set; }

        public Reservation Reservation { get; set; }
    }
}
