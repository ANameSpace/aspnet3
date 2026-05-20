using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.Attribute
{
    public class SentenceAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not string text)
            {
                return new ValidationResult("Текст должен быть строкой.");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult("Текст не может быть пустым или состоять из пробелов.");
            }

            if (!text.EndsWith('.'))
            {
                return new ValidationResult("Текст должен заканчиваться точкой.");
            }

            char firstChar = text[0];
            if (char.IsLetter(firstChar) && !char.IsUpper(firstChar))
            {
                return new ValidationResult("Первая буква текста должна быть заглавной.");
            }

            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (words.Length <= 3)
            {
                return new ValidationResult("Текст должен состоять более чем из трёх слов.");
            }

            return ValidationResult.Success;
        }
    }
}
