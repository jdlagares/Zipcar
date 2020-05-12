using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.DataAnnotations
{
    /// <summary>
    /// Valida que una fecha final no sea inferior a una fecha inicial
    /// </summary>
    public class EndDateValidation : ValidationAttribute
    {
        public string EndDate { get; set; }
        public string StartDate { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDateProp = validationContext.ObjectType.GetProperty(StartDate);
            var endDateProp = validationContext.ObjectType.GetProperty(EndDate);

            DateTime? startDate = startDateProp.GetValue(validationContext.ObjectInstance, null) as DateTime?;
            DateTime? endDate = endDateProp.GetValue(validationContext.ObjectInstance, null) as DateTime?;

            if (!startDate.HasValue && !endDate.HasValue)
            {
                return ValidationResult.Success;
            }

            if (startDate > endDate) 
            {
                return new ValidationResult(ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
