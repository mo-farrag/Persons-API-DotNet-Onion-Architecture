using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class PersonRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "blank")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "blank")]
        [Display(Name = "last_name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "blank")]
        public string CountryCode { get; set; }

        [Required(ErrorMessage = "blank")]
        [Phone(ErrorMessage = "not_a_number")]
        [MinLength(11, ErrorMessage = "too_short")]
        [MaxLength(14, ErrorMessage = "too_long")]
        public string Phone { get; set; }

        [GenderValues(ErrorMessage = "inclusion")]
        [Required(ErrorMessage = "blank")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "blank")]
        [DataType(DataType.Date, ErrorMessage = "Invalid Date")]
        [BirthDate(ErrorMessage = "in_the_future")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "blank")]
        [FileExtensionValidator(ErrorMessage = "invalid_content_type")]
        public string Avatar { get; set; }

        [EmailAddress(ErrorMessage = "invalid")]
        public string Email { get; set; }
    }

    public class BirthDateAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(ErrorMessage, new string[] { validationContext.DisplayName });

            DateTime _birthDate = Convert.ToDateTime(value);
            if (_birthDate < DateTime.Now)
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage, new string[] { validationContext.DisplayName });
        }
    }

    public class GenderValuesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(ErrorMessage, new string[] { validationContext.DisplayName });

            if (value.ToString().ToLower() == "male" || value.ToString().ToLower() == "female")
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage, new string[] { validationContext.DisplayName });
        }
    }

    public class FileExtensionValidator : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || value == string.Empty)
                return new ValidationResult(ErrorMessage, new string[] { validationContext.DisplayName });

            var data = value.ToString().Substring(0, 5);

            if (data == "IVBOR" || data == "/9j/4") //jpg, jpeg & png codes in the beginning of base64 string                
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage, new string[] { validationContext.DisplayName });

        }
    }

}
