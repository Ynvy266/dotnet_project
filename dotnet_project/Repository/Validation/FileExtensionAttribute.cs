using System.ComponentModel.DataAnnotations;

namespace dotnet_project.Repository.Validation
{
    public class FileExtensionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName);
                string[] extensions = { "png", "jpg", "jpeg" };

                bool result = extensions.Any(x => extension.EndsWith(x));

                if (!result)
                {
                    return new ValidationResult("Only files with the extensions png, jpg or jpeg are allowed.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
