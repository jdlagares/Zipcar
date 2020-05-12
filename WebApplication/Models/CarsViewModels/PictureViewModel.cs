using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.DataAnnotations;

namespace WebApplication.Models.CarsViewModels
{
    public class PictureViewModel
    {
        [Required]
        public long CarId { get; set; }

        [Required]
        [Display(Name = "Imagen")]
        [FileExtensionsValidation(Extensions = FileExtensionsValidation.JPEG + "," + FileExtensionsValidation.PNG)]
        [FileLenghtValidation(Lenght = 300000)]
        [ImageSize(IFromFileName = "File")]
        public IFormFile File { get; set; }
    }
}
