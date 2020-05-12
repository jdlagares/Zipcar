using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Enumerations
{
    public enum EnumCarType
    {
        [Display(Name = "Automovil")]
        Car,
        [Display(Name = "Camioneta")]
        Van
    }
}
