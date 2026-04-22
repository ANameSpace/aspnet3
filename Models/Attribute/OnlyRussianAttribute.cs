using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.Attribute
{
    public class OnlyRussianAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string text)
            {
                return ValidationResult.Success;
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return ValidationResult.Success;
            }

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (char.IsLetter(c) && !IsRussianLetter(c))
                {
                    return new ValidationResult("Все буквы в тексте должны быть русскими.");
                }
            }

            return ValidationResult.Success;
        }

        private static bool IsRussianLetter(char c)
        {
            return (c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я') || c == 'Ё' || c == 'ё';
        }
    }
}
