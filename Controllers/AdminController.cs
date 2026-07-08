using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using GaoChongPortfolio.Models;
using GaoChongPortfolio.Services;
using System.IO;
using System.Linq;

namespace GaoChongPortfolio.Controllers
{
    public class AdminController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public AdminController(IPortfolioService portfolioService, IConfiguration configuration, IWebHostEnvironment env)
        {
            _portfolioService = portfolioService;
            _configuration = configuration;
            _env = env;
        }

        // ==========================================
        // 🔐 AUTHENTICATION ENDPOINTS
        // ==========================================

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index");
            }
            return View(new AdminLoginModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var configUsername = _configuration["AdminSettings:Username"] ?? "admin";
            var configPassword = _configuration["AdminSettings:Password"] ?? "Admin123!";

            if (model.Username == configUsername && model.Password == configPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Username),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Invalid administrator credential payload. Verification failed.");
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ==========================================
        // 📊 DASHBOARD & BIO MANAGEMENT
        // ==========================================

        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            var data = _portfolioService.GetData();
            return View(data);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBio([Bind(Prefix = "Bio")] BioInfo model, IFormFile? AvatarFile)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Bio validation failed.";
                return RedirectToAction("Index");
            }

            var data = _portfolioService.GetData();

            if (AvatarFile != null && AvatarFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg" };
                var extension = Path.GetExtension(AvatarFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    TempData["Error"] = "Avatar upload failed. Invalid file format.";
                    return RedirectToAction("Index");
                }

                string uploadsDir;
                if (Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") != null)
                {
                    uploadsDir = "/home/site/uploads";
                }
                else
                {
                    var rootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                    uploadsDir = Path.Combine(rootPath, "images", "uploads");
                }

                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                var uniqueName = $"avatar-{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";
                var filePath = Path.Combine(uploadsDir, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await AvatarFile.CopyToAsync(stream);
                }

                model.AvatarUrl = $"/images/uploads/{uniqueName}";
            }
            else
            {
                // Preserve current AvatarUrl if not uploading a new file
                model.AvatarUrl = data.Bio.AvatarUrl;
            }

            data.Bio = model;
            _portfolioService.SaveData(data);

            TempData["Success"] = "Bio database metrics and avatar updated successfully!";
            return RedirectToAction("Index");
        }

        // ==========================================
        // 💼 TIMELINE EXPERIENCES CRUD
        // ==========================================

        [HttpGet]
        [Authorize]
        public IActionResult AddExperience()
        {
            return View("EditExperience", new ExperienceItem());
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditExperience(int id) // id acts as the index in the list
        {
            var data = _portfolioService.GetData();
            if (id < 0 || id >= data.Experiences.Count)
            {
                return NotFound("Experience index out of bounds.");
            }

            ViewBag.Index = id;
            return View(data.Experiences[id]);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult SaveExperience(ExperienceItem model, int? index, string bulletPointsRaw)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Index = index;
                return View("EditExperience", model);
            }

            // Split raw multi-line strings into list bullet points
            model.BulletPoints = string.IsNullOrWhiteSpace(bulletPointsRaw)
                ? new List<string>()
                : bulletPointsRaw.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                                 .Select(s => s.Trim())
                                 .ToList();

            var data = _portfolioService.GetData();

            if (index.HasValue && index.Value >= 0 && index.Value < data.Experiences.Count)
            {
                // Edit existing
                data.Experiences[index.Value] = model;
                TempData["Success"] = $"Experience at {model.Company} updated.";
            }
            else
            {
                // Add new
                data.Experiences.Add(model);
                TempData["Success"] = $"New experience at {model.Company} created.";
            }

            _portfolioService.SaveData(data);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteExperience(int id)
        {
            var data = _portfolioService.GetData();
            if (id >= 0 && id < data.Experiences.Count)
            {
                var company = data.Experiences[id].Company;
                data.Experiences.RemoveAt(id);
                _portfolioService.SaveData(data);
                TempData["Success"] = $"Experience at {company} removed.";
            }
            return RedirectToAction("Index");
        }

        // ==========================================
        // 🖼️ GALLERY PROJECTS CRUD WITH UPLOADS
        // ==========================================

        [HttpGet]
        [Authorize]
        public IActionResult AddProject()
        {
            return View("EditProject", new ProjectEditModel { Id = "", Category = "Web Projects" });
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProject(string id)
        {
            var data = _portfolioService.GetData();
            var project = data.Projects.FirstOrDefault(p => p.Id == id);
            if (project == null)
            {
                return NotFound("Project ID not registered.");
            }

            // Map standard model to Edit Model
            var editModel = new ProjectEditModel
            {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                ImageUrl = project.ImageUrl,
                Category = project.Category,
                TechStackRaw = string.Join(", ", project.TechStack),
                HumorousStat = project.HumorousStat
            };

            ViewBag.IsEdit = true;
            return View(editModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveProject(ProjectEditModel model, bool isEditMode)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.IsEdit = isEditMode;
                return View("EditProject", model);
            }

            var data = _portfolioService.GetData();
            var existingProject = data.Projects.FirstOrDefault(p => p.Id == model.Id);

            if (!isEditMode && existingProject != null)
            {
                ModelState.AddModelError("Id", "This Project Key is already occupied by another registry entity.");
                ViewBag.IsEdit = isEditMode;
                return View("EditProject", model);
            }

            // Handle Image File Uploads
            string finalImageUrl = model.ImageUrl ?? "/images/projects/portfolio.png";

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".gif", ".svg" };
                var extension = Path.GetExtension(model.ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Uploaded file must be a valid image format (.jpg, .png, .webp, .gif, .svg).");
                    ViewBag.IsEdit = isEditMode;
                    return View("EditProject", model);
                }

                // Create absolute uploads path in persistent directory
                string uploadsDir;
                if (Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") != null)
                {
                    uploadsDir = "/home/site/uploads";
                }
                else
                {
                    var rootPath = _env.WebRootPath ?? Path.Combine(_env.ContentRootPath, "wwwroot");
                    uploadsDir = Path.Combine(rootPath, "images", "uploads");
                }
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Generate safe, unique filename
                var uniqueName = $"{model.Id}-{Guid.NewGuid().ToString().Substring(0, 8)}{extension}";
                var filePath = Path.Combine(uploadsDir, uniqueName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                finalImageUrl = $"/images/uploads/{uniqueName}";
            }
            else if (isEditMode && existingProject != null)
            {
                // Fall back to pre-existing URL if no new file is uploaded
                finalImageUrl = existingProject.ImageUrl;
            }

            // Parse raw tags
            var techList = string.IsNullOrWhiteSpace(model.TechStackRaw)
                ? new List<string>()
                : model.TechStackRaw.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                   .Select(t => t.Trim())
                                   .ToList();

            var projectItem = new ProjectItem
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = finalImageUrl,
                Category = model.Category,
                TechStack = techList,
                HumorousStat = model.HumorousStat
            };

            if (isEditMode)
            {
                // Replace in list
                var index = data.Projects.FindIndex(p => p.Id == model.Id);
                if (index != -1)
                {
                    data.Projects[index] = projectItem;
                }
                TempData["Success"] = $"Project '{model.Title}' updated successfully.";
            }
            else
            {
                // Add new
                data.Projects.Add(projectItem);
                TempData["Success"] = $"New Project '{model.Title}' registered.";
            }

            _portfolioService.SaveData(data);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProject(string id)
        {
            var data = _portfolioService.GetData();
            var project = data.Projects.FirstOrDefault(p => p.Id == id);
            if (project != null)
            {
                data.Projects.Remove(project);
                _portfolioService.SaveData(data);
                TempData["Success"] = $"Project '{project.Title}' removed.";
            }
            return RedirectToAction("Index");
        }
    }
}
