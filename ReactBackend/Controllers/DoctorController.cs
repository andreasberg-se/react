using Microsoft.AspNetCore.Mvc;
using ReactBackend.Models;

namespace ReactBackend.Controllers
{

    public class DoctorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int temperature, string degrees)
        {
            ViewBag.Message = Doctor.CheckTemperature(temperature, degrees);
            ViewBag.Temperature = temperature;
            ViewBag.Degrees = degrees;
            return View();
        }
    }

}
