using System;
using Microsoft.AspNetCore.Mvc;
using ReactBackend.Models;
using ReactBackend.Models.ViewModels;
using ReactBackend.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace ReactBackend.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class PersonController : Controller
    {
        private readonly ReactBackendDbContext _reactBackendDbContext;

        public PersonController(ReactBackendDbContext reactBackendDbContext)
        {
            _reactBackendDbContext = reactBackendDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CityList"] = new SelectList(_reactBackendDbContext.Cities, "CityId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatePersonViewModel createPersonViewModel)
        {
            ViewData["CityList"] = new SelectList(_reactBackendDbContext.Cities, "CityId", "Name");

            if (!createPersonViewModel.IsValidForm())
                return View(createPersonViewModel);

            if (ModelState.IsValid)
            {
                Person person = new Person();
                person.FirstName = createPersonViewModel.FirstName;
                person.LastName = createPersonViewModel.LastName;
                person.CityId = createPersonViewModel.CityId;
                person.Phone = createPersonViewModel.Phone;

                 
                _reactBackendDbContext.People.Add(person);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(createPersonViewModel);
        }

        [HttpPost]
        public IActionResult Search(string searchString)
        {
            var result = from person in _reactBackendDbContext.People
                where person.FirstName.Contains(searchString.Trim())
                    || person.LastName.Contains(searchString.Trim())
                    //|| person.City.Contains(searchString.Trim()) 
                select person;

            try
            {
                return PartialView("~/Views/Person/Shared/_ListPartial.cshtml", result.ToList());
            }
            catch
            {
                return PartialView("~/Views/Person/Shared/_ListPartial.cshtml", _reactBackendDbContext.People.ToList());
            }    
        }

        [HttpGet]
        public IActionResult ListPeople()
        {
            return PartialView("~/Views/Person/Shared/_ListPartial.cshtml", _reactBackendDbContext.People.ToList());
        }

        [HttpGet]
        public IActionResult ShowPerson(int personId)
        {
            Person person = _reactBackendDbContext.People.Find(personId);
            if (person != null)
            {
                List<City> cities = _reactBackendDbContext.Cities.ToList();
                City city = cities.SingleOrDefault(ci => ci.CityId == person.CityId);
                ViewData["City"] = city.Name;

                List<Country> countries = _reactBackendDbContext.Countries.ToList();
                ViewData["Country"] = countries.SingleOrDefault(co => co.CountryId == city.CountryId).Name;
            }
            return PartialView("~/Views/Person/Shared/_DetailPartial.cshtml", person);
        }

        [HttpGet("{controller}/{action}/{personId}")]
        public IActionResult Edit(int personId)
        {
            ViewData["CityList"] = new SelectList(_reactBackendDbContext.Cities, "CityId", "Name");
            var editPerson = _reactBackendDbContext.People.Find(personId);

            if (editPerson != null)
            {
                EditPersonViewModel editPersonViewModel = new EditPersonViewModel();
                editPersonViewModel.PersonId = editPerson.PersonId;
                editPersonViewModel.FirstName = editPerson.FirstName;
                editPersonViewModel.LastName = editPerson.LastName;
                editPersonViewModel.CityId = editPerson.CityId;
                editPersonViewModel.Phone = editPerson.Phone;
                return View(editPersonViewModel);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditPersonViewModel editPersonViewModel)
        {
            ViewData["CityList"] = new SelectList(_reactBackendDbContext.Cities, "CityId", "Name");
            if ((!editPersonViewModel.IsValidForm()) || ((!ModelState.IsValid)))
                return View(editPersonViewModel);

            int personId = editPersonViewModel.PersonId;
            var editPerson = _reactBackendDbContext.People.Find(personId);
            if (editPerson != null)
            {
                editPerson.FirstName = editPersonViewModel.FirstName;
                editPerson.LastName = editPersonViewModel.LastName;
                editPerson.CityId = editPersonViewModel.CityId;
                editPerson.Phone = editPersonViewModel.Phone;
                _reactBackendDbContext.People.Update(editPerson);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction("Index", "Person");
            }
            
            ViewData["Message"] = "Failed to update person!";
            return View(editPersonViewModel);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeletePerson(int personId)
        {
            var deletePerson = _reactBackendDbContext.People.Find(personId);

            ViewBag.Message = "Failed to delete person!";
            if (deletePerson != null)
            {
                _reactBackendDbContext.People.Remove(deletePerson);
                _reactBackendDbContext.SaveChanges();
                ViewBag.Message = "The person was deleted successfully!";
            }     

            return PartialView("~/Views/Person/Shared/_DeletePartial.cshtml");
        }
    }

}
