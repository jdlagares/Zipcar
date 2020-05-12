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
    /// Valida que un archivo IFormFile tenga la extension correspondiente
    /// </summary>
    public class FileExtensionsValidation : ValidationAttribute
    {
        public const string HTML = ".html";
        public const string PNG = ".png";
        public const string JPEG = ".jpg";
        public const string CSV = ".csv";

        public string Extensions;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string[] split = Extensions.Split(',');
            if (value != null)
            {
                var file = (IFormFile) value;
                string extension = Path.GetExtension(file.FileName);

                bool success = false;
                foreach (var item in split)
                {
                    if (extension == item)
                    {
                        success = true;
                    }
                }

                if (!success)
                {
                    return new ValidationResult($"solo se acepta {string.Join(" ", split)}");
                }

                return ValidationResult.Success;
            }

            return new ValidationResult("Error Inesperado");
        }
    }
}
