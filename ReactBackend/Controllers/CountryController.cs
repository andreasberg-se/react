using System;
using Microsoft.AspNetCore.Mvc;
using ReactBackend.Models;
using ReactBackend.Models.ViewModels;
using ReactBackend.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ReactBackend.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class CountryController : Controller
    {
        private readonly ReactBackendDbContext _reactBackendDbContext;

        public CountryController(ReactBackendDbContext reactBackendDbContext)
        {
            _reactBackendDbContext = reactBackendDbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Index(CountryViewModel countryViewModel)
        {
            if (!countryViewModel.IsValidForm())
                return View(countryViewModel);

            if (ModelState.IsValid)
            {
                Country country = new Country();
                country.Name = countryViewModel.Name;
                
                _reactBackendDbContext.Countries.Add(country);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            return View(countryViewModel);
        }

        [HttpGet]
        public IActionResult Show()
        {
            return View(_reactBackendDbContext.Countries.ToList());
        }

        [HttpGet("{controller}/{action}/{countryId}")]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Edit(int countryId)
        {
            var editCountry = _reactBackendDbContext.Countries.Find(countryId);

            if (editCountry != null)
            {
                EditCountryViewModel editCountryViewModel = new EditCountryViewModel();
                editCountryViewModel.CountryId = editCountry.CountryId;
                editCountryViewModel.Name = editCountry.Name;
                return View(editCountryViewModel);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Edit(EditCountryViewModel editCountryViewModel)
        {
            if ((!editCountryViewModel.IsValidForm()) || ((!ModelState.IsValid)))
                return View(editCountryViewModel);

            int countryId = editCountryViewModel.CountryId;
            var editCountry = _reactBackendDbContext.Countries.Find(countryId);
            if (editCountry != null)
            {
                editCountry.Name = editCountryViewModel.Name;
                _reactBackendDbContext.Countries.Update(editCountry);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction("Index", "Person");
            }
            
            ViewData["Message"] = "Failed to update country!";
            return View(editCountryViewModel);
        }

        [HttpGet("{controller}/{action}/{countryId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Show(int countryId)
        {
            var deleteCountry = _reactBackendDbContext.Countries.FirstOrDefault(co => co.CountryId == countryId);
            if (deleteCountry == null)
            {
                ViewData["Message"] = "Failed to delete country (not found)!";
                return View(_reactBackendDbContext.Countries.ToList());
            }

            try
            {
                _reactBackendDbContext.Countries.Remove(deleteCountry);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            catch
            {
                ViewData["Message"] = $"Failed to delete {deleteCountry.Name}!";
            }
            return View(_reactBackendDbContext.Countries.ToList());
        }
    }
}