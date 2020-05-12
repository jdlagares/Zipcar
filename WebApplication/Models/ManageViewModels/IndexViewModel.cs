using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.ManageViewModels
{
    public class IndexViewModel
    {
        [Display(Name = "Nombre")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        [Required]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Nit/Cedula")]
        public string Identification { get; set; }

        [Phone]
        [Display(Name = "Telefono")]
        [Required]
        public string PhoneNumber { get; set; }

        public string StatusMessage { get; set; }
    }
}
