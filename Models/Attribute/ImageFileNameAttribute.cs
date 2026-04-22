using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.Attribute
{
    public class ImageFileNameAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || value is not string fileName)
            {
                return ValidationResult.Success;
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return new ValidationResult("Название файла не может быть пустым.");
            }

            if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Файл должен иметь расширение .png или .jpg.");
        }
    }
}
