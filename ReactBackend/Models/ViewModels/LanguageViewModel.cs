using System.ComponentModel.DataAnnotations;

namespace ReactBackend.Models.ViewModels
{

    public class LanguageViewModel
    {
        [Required]
        [Display(Name="Language")]
        [StringLength(100, MinimumLength = 1)]
        public string LanguageName { get; set; }

        public bool IsValidForm()
        {
            return (!string.IsNullOrWhiteSpace(LanguageName));
        }
    }

}