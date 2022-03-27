using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ReactBackend.Models
{

    public class Country
    {
        [Key]
        public int CountryId { get; set; }

        [Required]
        public string Name { get; set; }

        public List<City> Cities { get; set; }
    }

}
