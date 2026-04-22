using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.Attribute
{
    public class NotFutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is not DateTime date)
            {
                return new ValidationResult("Некорректный тип данных.");
            }

            if (date.Date > DateTime.Today)
            {
                return new ValidationResult("Дата не может быть позже сегодняшнего дня.");
            }

            return ValidationResult.Success;
        }
    }
}
