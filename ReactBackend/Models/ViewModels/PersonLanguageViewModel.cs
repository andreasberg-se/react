using System.Collections.Generic;

namespace ReactBackend.Models.ViewModels
{

    public class PersonLanguageViewModel
    {
        public List<Person> People { get; set; }
        public List<Language> Languages { get; set; }
        public List<PersonLanguage> PersonLanguages { get; set; }
    }

}
