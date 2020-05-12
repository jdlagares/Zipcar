using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Enumerations;

namespace WebApplication.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = "La marca es requerida")]
        public string Brand { get; set; }

        public string Picture { get; set; }

        [Display(Name = "Año")]
        [Required(ErrorMessage = "El año es requerido")]
        public short Year { get; set; }

        [Display(Name = "Modelo")]
        [Required(ErrorMessage = "El modelo es requerido")]
        public string Model { get; set; }

        [Display(Name = "Capacidad")]
        [Required(ErrorMessage = "La capacidad el carro es requerida")]
        public short Capacity { get; set; }

        [Display(Name = "Matricula")]
        [Required(ErrorMessage = "La matricula del carro es requerida")]
        public string CarCode { get; set; }

        [Display(Name = "Precio por dia")]
        [Required(ErrorMessage = "El precio por dia es requerido")]
        public decimal PricePerDay { get; set; }

        [Display(Name = "Precio por mes")]
        [Required(ErrorMessage = "El precio por mes es requerido")]
        public decimal PricePerMonth { get; set; }

        [Display(Name = "Precio por año")]
        [Required(ErrorMessage = "El precio por año es requerido")]
        public decimal PricePerYear { get; set; }

        [Display(Name = "Tipo de carro")]
        [Required(ErrorMessage = "El tipo de carro es requerido")]
        public EnumCarType CarType { get; set; }

        [Display(Name = "Parqueadero")]
        [Required(ErrorMessage = "El parqueadero es requerido")]
        public int ParkingId { get; set; }

        public Parking Parking { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
