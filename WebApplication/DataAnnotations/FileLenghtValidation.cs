using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAnnotations
{
    /// <summary>
    /// Valida que una archivo IFormFile tenga el tamaño correspondiente en bytes
    /// </summary>
    public class FileLenghtValidation : ValidationAttribute
    { 
        public long Lenght; 

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var file = (IFormFile) value;

                if (!(file.Length > 0))
                {
                    return new ValidationResult($"El archivo no tiene contenido");
                }
                if (file.Length > Lenght)
                {
                    return new ValidationResult($"Solo se aceptan archivos con tamaños inferiores o iguales a {Lenght} bytes");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Error Inesperado");
        }
    }
}
