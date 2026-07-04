using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GaoChongPortfolio.Models;
using GaoChongPortfolio.Services;

namespace GaoChongPortfolio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPortfolioService _portfolioService;

        public HomeController(ILogger<HomeController> logger, IPortfolioService portfolioService)
        {
            _logger = logger;
            _portfolioService = portfolioService;
        }

        public IActionResult Index()
        {
            var data = _portfolioService.GetData();
            return View(data.Bio);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int? statusCode = null)
        {
            ViewData["StatusCode"] = statusCode;
            ViewData["RequestId"] = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            if (statusCode == 404)
            {
                ViewData["ErrorTitle"] = "404 - Sitemap Index Missing";
                ViewData["ErrorMessage"] = "The server searched deep inside the recursive namespaces and couldn't find the resource you requested. It is highly likely that a NullReferenceException was thrown, or you typed a typo in the URL.";
                return View("Error");
            }

            ViewData["ErrorTitle"] = "500 - Semicolon Stack Overflow";
            ViewData["ErrorMessage"] = "Our background garbage collector crashed. Don't worry, the developer has been notified (or is currently making coffee).";
            return View("Error");
        }
    }
}
