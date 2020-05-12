using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;

namespace WebApplication.DataAnnotations
{
    /// <summary>
    /// Valida que una imagen tenga un altura y anchura especificada
    /// </summary>
    public class ImageSize : ValidationAttribute
    {
        public string IFromFileName { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ImageProp = validationContext.ObjectType.GetProperty(IFromFileName);
            var formFile = (IFormFile) ImageProp.GetValue(validationContext.ObjectInstance, null);

            if (formFile != null && formFile.Length > 0)
            {
                try
                {
                    using (var image = SKBitmap.Decode(formFile.OpenReadStream())) 
                    {
                        if (value != null)
                        {
                            if (image.Height != image.Width)
                            {
                                return new ValidationResult($"Error la imagen debe ser de cuadrada, relacion 1:1");
                            }

                            return ValidationResult.Success;
                        }
                    }
                }
                catch (Exception e)
                {
                    return new ValidationResult(e.Message);
                }
            }

            return new ValidationResult("No selecciono una imagen");
        }
    }
}
