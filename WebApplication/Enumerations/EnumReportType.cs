using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Enumerations
{
    public enum EnumReportType
    {
        [Display(Name = "Accidente")]
        Accident,
        [Display(Name = "Fallas del auto")]
        CarDamage,
        [Display(Name = "Otros")]
        Others
    }
}
