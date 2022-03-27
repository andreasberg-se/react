using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ReactBackend.Models
{

    public class Language
    {
        [Key]
        public int LanguageId { get; set; }

        [Required]
        public string LanguageName { get; set; }

        public List<PersonLanguage> PersonLanguages { get; set; }
    }

}
