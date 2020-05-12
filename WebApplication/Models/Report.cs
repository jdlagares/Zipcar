using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Enumerations;

namespace WebApplication.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Display(Name = "Resuelto")]
        public bool Resolved { get; set; }

        [Display(Name = "Tipo")]
        public EnumReportType Type { get; set; }

        [Display(Name = "Fecha")]
        public DateTime Date { get; set; }

        [Display(Name = "Descripcion")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Reserva")]
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
