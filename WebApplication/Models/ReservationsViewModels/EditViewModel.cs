using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.ReservationsViewModels
{
    public class EditViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Fecha final real")]
        public DateTime RealDateEnd { get; set; }
    }
}
