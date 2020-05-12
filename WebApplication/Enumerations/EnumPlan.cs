using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Enumerations
{
    public enum EnumPlan
    {
        [Display(Name = "Plan diario")]
        PerDay,
        [Display(Name = "Plan mensual")]
        PerMonth,
        [Display(Name = "Plan anual")]
        PerYear
    }
}
