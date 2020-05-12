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

        [Display(Name = "Carro")]
        public int CarId { get; set; }

        public Car Car { get; set; }

        [Display(Name = "Plan")]
        public EnumPlan Plan { get; set; }

        [Display(Name = "Fecha inicial")]
        public DateTime DateStart { get; set; }

        [Display(Name = "Fecha final")]
        public DateTime EstimateDateEnd { get; set; }

        [Display(Name = "Fecha final real")]
        public DateTime? RealDateEnd { get; set; }

        public string ApplicationUserId { get; set; }

        [Required]
        public ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Fue retornado")]
        public bool Returned { get; set; }

        [Display(Name = "Fue entregado")]
        public bool Delivered { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Report> Reports { get; set; }

    }
}
