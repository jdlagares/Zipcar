﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

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
        public string PhoneNumber { get; set; }
    }
}
