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
    public class PersonLanguageController : Controller
    {
        private readonly ReactBackendDbContext _reactBackendDbContext;

        public PersonLanguageController(ReactBackendDbContext reactBackendDbContext)
        {
            _reactBackendDbContext = reactBackendDbContext;
        }

        public IActionResult Index()
        {
            ViewData["PeopleList"] = new SelectList(_reactBackendDbContext.People, "PersonId", "FirstName");
            ViewData["LanguageList"] = new SelectList(_reactBackendDbContext.Languages, "LanguageId", "LanguageName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Index(PersonLanguage personLanguage)
        {
            ViewData["PeopleList"] = new SelectList(_reactBackendDbContext.People, "PersonId", "FirstName");
            ViewData["LanguageList"] = new SelectList(_reactBackendDbContext.Languages, "LanguageId", "LanguageName");
            if (ModelState.IsValid)
            {
                try
                {
                    _reactBackendDbContext.PersonLanguages.Add(personLanguage);
                    _reactBackendDbContext.SaveChanges();
                    return RedirectToAction(nameof(Index), "Person");
                }
                catch
                {
                    ViewData["Message"] = "Failed to add language to person!";
                }
            }
            return View(personLanguage);
        }

        [HttpGet]
        public IActionResult Show()
        {
            PersonLanguageViewModel personLanguageViewModel = new PersonLanguageViewModel();
            personLanguageViewModel.People = _reactBackendDbContext.People.ToList();
            personLanguageViewModel.Languages = _reactBackendDbContext.Languages.ToList();
            personLanguageViewModel.PersonLanguages = _reactBackendDbContext.PersonLanguages.ToList();
            return View(personLanguageViewModel);
        }

        [HttpGet("{controller}/{action}/{pid}/{lid}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Show(int pid, int lid)
        {
            PersonLanguageViewModel personLanguageViewModel = new PersonLanguageViewModel();
            personLanguageViewModel.People = _reactBackendDbContext.People.ToList();
            personLanguageViewModel.Languages = _reactBackendDbContext.Languages.ToList();
            personLanguageViewModel.PersonLanguages = _reactBackendDbContext.PersonLanguages.ToList();

            var deletePersonLanguage = _reactBackendDbContext.PersonLanguages.FirstOrDefault(pl => pl.PersonId == pid && pl.LanguageId == lid);
            if (deletePersonLanguage == null)
            {
                ViewData["Message"] = "Failed to delete spoken language (not found)!";
                return View(personLanguageViewModel);
            }

            try
            {
                _reactBackendDbContext.PersonLanguages.Remove(deletePersonLanguage);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            catch
            {
                ViewData["Message"] = "Failed to delete spoken language!";
            }
            return View(personLanguageViewModel);
        }
    }

}