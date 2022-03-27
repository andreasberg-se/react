using System.ComponentModel.DataAnnotations;

namespace ReactBackend.Models.ViewModels
{

    public class CountryViewModel
    {
        [Required]
        [Display(Name="Country name")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        public bool IsValidForm()
        {
            return (!string.IsNullOrWhiteSpace(Name));
        }
    }

}