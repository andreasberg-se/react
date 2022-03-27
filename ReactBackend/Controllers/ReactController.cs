using Microsoft.AspNetCore.Mvc;
using ReactBackend.Models;
using ReactBackend.Models.DTO;
using ReactBackend.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;

namespace ReactBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReactController : Controller
    {
        private readonly ReactBackendDbContext _reactBackendDbContext;

        public ReactController(ReactBackendDbContext reactBackendDbContext)
        {
            _reactBackendDbContext = reactBackendDbContext;
        }

        // GET: api/react
        [HttpGet]
        public string Get()
        {
            List<Person> People = _reactBackendDbContext.People.ToList();
            List<City> Cities = _reactBackendDbContext.Cities.ToList();
            List<Country> Countries = _reactBackendDbContext.Countries.ToList();
            List<Language> Languages = _reactBackendDbContext.Languages.ToList();
            List<PersonLanguage> PersonLanguages = _reactBackendDbContext.PersonLanguages.ToList();
            List<PersonDTO> PeopleList = new List<PersonDTO>();

            foreach (Person person in People)
            {
                PersonDTO personDTO = new PersonDTO();
                personDTO.PersonId = person.PersonId;
                personDTO.FirstName = person.FirstName;
                personDTO.LastName = person.LastName;
                City city = Cities.Where(ci => ci.CityId == person.CityId).SingleOrDefault();
                personDTO.City = city.Name;
                Country country = Countries.Where(co => co.CountryId == city.CountryId).SingleOrDefault();
                personDTO.Country = country.Name;
                string languages = "";
                foreach(PersonLanguage personLanguage in PersonLanguages)
                {
                    if (person.PersonId == personLanguage.PersonId)
                    {
                        Language language = Languages.Where(l => l.LanguageId == personLanguage.LanguageId).SingleOrDefault();
                        if (languages.Length > 0) languages += ", ";
                        languages += language.LanguageName;
                    }
                }
                personDTO.Languages = languages;
                PeopleList.Add(personDTO);
            }

            string JsonData = JsonSerializer.Serialize(PeopleList);
            return JsonData;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            List<Person> People = _reactBackendDbContext.People.ToList();
            List<City> Cities = _reactBackendDbContext.Cities.ToList();
            List<Country> Countries = _reactBackendDbContext.Countries.ToList();
            List<Language> Languages = _reactBackendDbContext.Languages.ToList();
            List<PersonLanguage> PersonLanguages = _reactBackendDbContext.PersonLanguages.ToList();
            List<PersonDTO> PeopleList = new List<PersonDTO>();

            Person person = People.Where(p => p.PersonId == id).SingleOrDefault();
            if (person == null)
                return null;

            PersonDTO personDTO = new PersonDTO();
            personDTO.PersonId = person.PersonId;
            personDTO.FirstName = person.FirstName;
            personDTO.LastName = person.LastName;
            City city = Cities.Where(ci => ci.CityId == person.CityId).SingleOrDefault();
            personDTO.City = city.Name;
            Country country = Countries.Where(co => co.CountryId == city.CountryId).SingleOrDefault();
            personDTO.Country = country.Name;
            string languages = "";
            foreach(PersonLanguage personLanguage in PersonLanguages)
            {
                if (person.PersonId == personLanguage.PersonId)
                {
                    Language language = Languages.Where(l => l.LanguageId == personLanguage.LanguageId).SingleOrDefault();
                    if (languages.Length > 0) languages += ", ";
                    languages += language.LanguageName;
                }
            }
            personDTO.Languages = languages;            

            string JsonData = JsonSerializer.Serialize(personDTO);
            return JsonData;
        }

        [HttpGet("cities")]
        public string GetCities()
        {
            List<City> Cities = _reactBackendDbContext.Cities.ToList();
            string JsonData = JsonSerializer.Serialize(Cities);
            return JsonData;
        }

        [HttpPost]
        public void Post([FromBody] Person value)
        {
            if (value == null)
                return;

            _reactBackendDbContext.People.Add(value);
            _reactBackendDbContext.SaveChanges();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            Person person = _reactBackendDbContext.People.Where(p => p.PersonId == id).SingleOrDefault();
            if (person != null)
            {
                _reactBackendDbContext.People.Remove(person);
                _reactBackendDbContext.SaveChanges();
            }
        }
    }

}