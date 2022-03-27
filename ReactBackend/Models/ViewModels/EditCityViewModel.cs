using System.ComponentModel.DataAnnotations;

namespace ReactBackend.Models.ViewModels
{

    public class EditCityViewModel
    {
        [Required]
        public int CityId { get; set; }
        
        [Required]
        [Display(Name="City name")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Display(Name="Country")]
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public bool IsValidForm()
        {
            return (!string.IsNullOrWhiteSpace(Name));
        }
    }

}