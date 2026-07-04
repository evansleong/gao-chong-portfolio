using Microsoft.AspNetCore.Mvc;
using GaoChongPortfolio.Models;
using GaoChongPortfolio.Services;

namespace GaoChongPortfolio.Controllers
{
    public class GalleryController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public GalleryController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public IActionResult Index(string category = "All")
        {
            var data = _portfolioService.GetData();
            var projects = data.Projects;
            
            var categories = new List<string> { "All", "Web Projects", "Cloud Infrastructure", "Hardware Builds", "Automotive Tinkering" };

            var filteredProjects = category.Equals("All", StringComparison.OrdinalIgnoreCase)
                ? projects
                : projects.Where(p => p.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();

            var viewModel = new ProjectViewModel
            {
                Projects = filteredProjects,
                Categories = categories,
                ActiveCategory = category
            };

            return View(viewModel);
        }
    }
}
