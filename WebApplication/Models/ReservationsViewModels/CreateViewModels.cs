using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Enumerations;

namespace WebApplication.Models.ReservationsViewModels
{
    public class CreateViewModels
    {
        [Display(Name = "Carro")]
        public int CarId { get; set; }

        [Display(Name = "Plan")]
        public EnumPlan Plan { get; set; }

        [Display(Name = "Fecha inicial")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Fecha final")]
        [DataAnnotations.EndDateValidation(StartDate = "DateStart", EndDate = "EstimateDateEnd", ErrorMessage = "La fecha final debe ser superior a la inicial")]
        public DateTime EstimateDateEnd { get; set; }
    }
}
