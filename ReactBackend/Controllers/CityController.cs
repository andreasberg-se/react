using System;
using Microsoft.AspNetCore.Mvc;
using ReactBackend.Models;
using ReactBackend.Models.ViewModels;
using ReactBackend.Data;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace ReactBackend.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class CityController : Controller
    {
        private readonly ReactBackendDbContext _reactBackendDbContext;

        public CityController(ReactBackendDbContext reactBackendDbContext)
        {
            _reactBackendDbContext = reactBackendDbContext;
        }

        public IActionResult Index()
        {
            ViewData["CountryList"] = new SelectList(_reactBackendDbContext.Countries, "CountryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Index(CityViewModel cityViewModel)
        {
            ViewData["CountryList"] = new SelectList(_reactBackendDbContext.Countries, "CountryId", "Name");

            if (!cityViewModel.IsValidForm())
                return View(cityViewModel);

            if (ModelState.IsValid)
            {
                City city = new City();
                city.Name = cityViewModel.Name;
                city.CountryId = cityViewModel.CountryId;
                
                _reactBackendDbContext.Cities.Add(city);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            return View(cityViewModel);
        }

        [HttpGet]
        public IActionResult Show()
        {
            return View(_reactBackendDbContext.Cities.ToList());
        }

        [HttpGet("{controller}/{action}/{cityId}")]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Edit(int cityId)
        {
            ViewData["CountryList"] = new SelectList(_reactBackendDbContext.Countries, "CountryId", "Name");
            var editCity = _reactBackendDbContext.Cities.Find(cityId);

            if (editCity != null)
            {
                EditCityViewModel editCityViewModel = new EditCityViewModel();
                editCityViewModel.CityId = editCity.CityId;
                editCityViewModel.Name = editCity.Name;
                editCityViewModel.CountryId = editCity.CountryId;
                return View(editCityViewModel);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Edit(EditCityViewModel editCityViewModel)
        {
            ViewData["CountryList"] = new SelectList(_reactBackendDbContext.Countries, "CountryId", "Name");
            if ((!editCityViewModel.IsValidForm()) || ((!ModelState.IsValid)))
                return View(editCityViewModel);

            int cityId = editCityViewModel.CityId;
            var editCity = _reactBackendDbContext.Cities.Find(cityId);
            if (editCity != null)
            {
                editCity.Name = editCityViewModel.Name;
                editCity.CountryId = editCityViewModel.CountryId;
                _reactBackendDbContext.Cities.Update(editCity);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction("Index", "Person");
            }
            
            ViewData["Message"] = "Failed to update city!";
            return View(editCityViewModel);
        }

        [HttpGet("{controller}/{action}/{cityId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Show(int cityId)
        {
            var deleteCity = _reactBackendDbContext.Cities.FirstOrDefault(ci => ci.CityId == cityId);
            if (deleteCity == null)
            {
                ViewData["Message"] = "Failed to delete city (not found)!";
                return View(_reactBackendDbContext.Cities.ToList());
            }

            try
            {
                _reactBackendDbContext.Cities.Remove(deleteCity);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            catch
            {
                ViewData["Message"] = $"Failed to delete {deleteCity.Name}!";
            }
            return View(_reactBackendDbContext.Cities.ToList());
        }
    }
}