using Microsoft.AspNetCore.Mvc;
using GaoChongPortfolio.Models;

namespace GaoChongPortfolio.Controllers
{
    public class ContactController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ContactViewModel model)
        {
            // Honeypot anti-spam check
            if (!string.IsNullOrEmpty(model.Honeypot))
            {
                // Pretend to be successful to confuse the bot, but log a warning.
                TempData["HoneypotTriggered"] = true;
                TempData["SubmitterName"] = "System.Bot";
                return RedirectToAction("Success");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Real message submission logic (could be saving to DB or sending email)
            // For now, we pass details to TempData for the success screen.
            TempData["HoneypotTriggered"] = false;
            TempData["SubmitterName"] = model.Name;

            return RedirectToAction("Success");
        }

        [HttpGet]
        public IActionResult Success()
        {
            var submitterName = TempData["SubmitterName"] as string ?? "Guest Recruiter";
            var botTriggered = TempData["HoneypotTriggered"] as bool? ?? false;

            ViewData["Submitter"] = submitterName;
            ViewData["IsBot"] = botTriggered;

            return View();
        }
    }
}
