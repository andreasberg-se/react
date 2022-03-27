using Microsoft.AspNetCore.Mvc;
using ReactBackend.Models;
using ReactBackend.Models.ViewModels;
using ReactBackend.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace ReactBackend.Controllers
{
    [Authorize(Roles = "Admin, Moderator, User")]
    public class LanguageController : Controller
    {
        private readonly ReactBackendDbContext _reactBackendDbContext;

        public LanguageController(ReactBackendDbContext reactBackendDbContext)
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
        public IActionResult Index(LanguageViewModel languageViewModel)
        {
            if (!languageViewModel.IsValidForm())
                return View(languageViewModel);

            if (ModelState.IsValid)
            {
                Language language = new Language();
                language.LanguageName = languageViewModel.LanguageName;
                
                _reactBackendDbContext.Languages.Add(language);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            return View(languageViewModel);
        }

        [HttpGet]
        public IActionResult Show()
        {
            return View(_reactBackendDbContext.Languages.ToList());
        }

        [HttpGet("{controller}/{action}/{languageId}")]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Edit(int languageId)
        {
            var editLanguage = _reactBackendDbContext.Languages.Find(languageId);

            if (editLanguage != null)
            {
                EditLanguageViewModel editLanguageViewModel = new EditLanguageViewModel();
                editLanguageViewModel.LanguageId = editLanguage.LanguageId;
                editLanguageViewModel.LanguageName = editLanguage.LanguageName;
                return View(editLanguageViewModel);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Edit(EditLanguageViewModel editLanguageViewModel)
        {
            if ((!editLanguageViewModel.IsValidForm()) || ((!ModelState.IsValid)))
                return View(editLanguageViewModel);

            int languageId = editLanguageViewModel.LanguageId;
            var editLanguage = _reactBackendDbContext.Languages.Find(languageId);
            if (editLanguage != null)
            {
                editLanguage.LanguageName = editLanguageViewModel.LanguageName;
                _reactBackendDbContext.Languages.Update(editLanguage);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction("Index", "Person");
            }
            
            ViewData["Message"] = "Failed to update language!";
            return View(editLanguageViewModel);
        }

        [HttpGet("{controller}/{action}/{languageId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Show(int languageId)
        {
            var deleteLanguage = _reactBackendDbContext.Languages.FirstOrDefault(l => l.LanguageId == languageId);
            if (deleteLanguage == null)
            {
                ViewData["Message"] = "Failed to delete language (not found)!";
                return View(_reactBackendDbContext.Languages.ToList());
            }

            try
            {
                _reactBackendDbContext.Languages.Remove(deleteLanguage);
                _reactBackendDbContext.SaveChanges();
                return RedirectToAction(nameof(Index), "Person");
            }
            catch
            {
                ViewData["Message"] = $"Failed to delete {deleteLanguage.LanguageName}!";
            }
            return View(_reactBackendDbContext.Languages.ToList());
        }
    }
    
}