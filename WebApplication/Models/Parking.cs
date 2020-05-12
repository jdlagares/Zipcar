using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models
{
    public class Parking
    {
        public int Id { get; set; }

        [Display(Name = "Direccion del parqueadero")]
        [Required(ErrorMessage = "La direccion del parqueadero es requerida")] 
        public string Address { get; set; }

        public virtual ICollection<Car> Cars { get; set; }
    }
}
