using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.Attribute
{
    public class NameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string name)
            {
                return new ValidationResult("Название должно быть строкой.");
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                return new ValidationResult("Название не может быть пустым или состоять из пробелов.");
            }

            if (name.Length <= 3)
            {
                return new ValidationResult("Название должно содержать более 3 символов.");
            }

            if (char.IsDigit(name.ElementAt(0)))
            {
                return new ValidationResult("Название не может начинаться с цифры");
            }

            return ValidationResult.Success;
        }
    }
}
