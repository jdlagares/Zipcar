using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.CarsViewModels
{
    public class SearchViewModel
    {
        [Display(Name = "Marca")]
        public string Brand { get; set; }

        [Display(Name = "Año")]
        public short? Year { get; set; }

        [Display(Name = "Capacidad")]
        public short? Capacity { get; set; }

        [Display(Name = "Modelo")]
        public string Model { get; set; }

        [Display(Name = "Fecha inicio")]
        [DataType(DataType.Date)]
        public DateTime? Start { get; set; }

        [Display(Name = "Fecha final")]
        [DataType(DataType.Date)]
        public DateTime? End { get; set; }
    }
}
