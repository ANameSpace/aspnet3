using System.ComponentModel.DataAnnotations;

namespace PSite3.Models.Attribute
{
    public class CensorshipAttribute : ValidationAttribute
    {
        private readonly string[] BadWord = ["vpn", "telegram", "tg", "telega", "facebook", "whatsapp", "instagrasm"];

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            foreach (var word in BadWord)
            {
                if (value is string str && str.Contains(word, StringComparison.OrdinalIgnoreCase))
                {
                    return new ValidationResult("Текст содержит запрещённое слово.");
                }

            }

            return ValidationResult.Success;
        }
    }
}
