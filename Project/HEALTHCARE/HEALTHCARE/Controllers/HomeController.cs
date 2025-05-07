using System.Diagnostics;
using HEALTHCARE.Models; // C?n thi?t n?u ErrorViewModel n?m ? ?�y
using Microsoft.AspNetCore.Mvc;

namespace HEALTHCARE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Action Index n�y ch? tr? v? View, kh�ng k�m theo Model ph?c t?p n�o.
        // ?i?u n�y l� ?�NG cho m?t trang ch? th�ng th??ng.
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}