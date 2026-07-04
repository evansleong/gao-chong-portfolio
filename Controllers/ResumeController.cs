using Microsoft.AspNetCore.Mvc;
using GaoChongPortfolio.Models;
using GaoChongPortfolio.Services;
using System.Text;

namespace GaoChongPortfolio.Controllers
{
    public class ResumeController : Controller
    {
        private readonly IPortfolioService _portfolioService;

        public ResumeController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        public IActionResult Index()
        {
            var data = _portfolioService.GetData();
            
            // Build the view model dynamically
            var viewModel = new ResumeViewModel
            {
                Experiences = data.Experiences,
                SkillGroups = GetSkillGroups(), // Skills can remain statically grouped or loaded
                Certifications = GetCertifications()
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult TerminalCommand(string cmd)
        {
            if (string.IsNullOrWhiteSpace(cmd))
            {
                return Json(new { output = "Error: Empty command payload." });
            }

            var cleanCmd = cmd.Trim().ToLower();
            string response;
            var data = _portfolioService.GetData();

            switch (cleanCmd)
            {
                case "help":
                    response = "Available commands:\r\n  help        - Display this menu\r\n  bio         - Print developer profile description\r\n  skills      - Query technical skills array\r\n  experience  - List work experience data dynamically\r\n  hire        - Initiate onboarding sequence\r\n  matrix      - Trigger retro overlay\r\n  sudo rm -rf / - Wipe root partition";
                    break;
                case "bio":
                    response = $"Gao Chong - Senior Full-Stack Engineer\r\n---------------------------------------\r\nSpecialty: High-throughput backend C# microservices, AWS serverless pipelines.\r\nBio Summary: {data.Bio.HeroSubtitle}";
                    break;
                case "skills":
                    response = "System Skills Query Results:\r\n  * Languages: C#, Python, JavaScript, Dart, SQL, HTML/CSS\r\n  * Frameworks: ASP.NET Core MVC, React, Flutter, Sitecore (10.3/10.4)\r\n  * Cloud: AWS Lambda, Rekognition, DynamoDB, Cognito, S3\r\n  * Tools: ILSpy, Log Analyzer, Git, Docker, IIS";
                    break;
                case "experience":
                    var sb = new StringBuilder();
                    sb.AppendLine("Dynamic Work History Query Results:");
                    sb.AppendLine("===================================");
                    for (int i = 0; i < data.Experiences.Count; i++)
                    {
                        var exp = data.Experiences[i];
                        sb.AppendLine($"  [{i + 1}] {exp.Role} at {exp.Company}");
                        sb.AppendLine($"      Duration: {exp.Duration}");
                        if (exp.BulletPoints.Count > 0)
                        {
                            sb.AppendLine($"      Key: {exp.BulletPoints[0]}");
                        }
                        sb.AppendLine();
                    }
                    response = sb.ToString();
                    break;
                case "hire":
                    response = "[SUCCESS] Dispatching interview invite... \r\nSystem Response: 'Salary expectation satisfies developer.OnboardingRequested = true;'\r\nLooking forward to speaking with you!";
                    break;
                case "matrix":
                    response = "[SYS] Retro mode override active. (Or click the floating green pixel at the top-right corner to toggle it).";
                    break;
                case "sudo rm -rf /":
                    response = "WARNING: Access Denied. Gao Chong's kernel is compiled with write-protection. Nice try, hacker.";
                    break;
                default:
                    response = $"Command '{cmd}' not found. Type 'help' for a list of valid commands.";
                    break;
            }

            return Json(new { output = response });
        }

        private List<SkillGroup> GetSkillGroups()
        {
            return new List<SkillGroup>
            {
                new SkillGroup
                {
                    CategoryName = "Languages",
                    Skills = new List<string> { "C#", "Python", "JavaScript", "TypeScript", "Dart", "SQL", "HTML5/CSS3" }
                },
                new SkillGroup
                {
                    CategoryName = "Frameworks & Architectures",
                    Skills = new List<string> { "ASP.NET Core MVC", "React", "Flutter", "Sitecore 10.x", "Entity Framework Core", "RESTful APIs" }
                },
                new SkillGroup
                {
                    CategoryName = "Cloud & Devops",
                    Skills = new List<string> { "AWS (Lambda, Rekognition, Cognito, DynamoDB, S3)", "Docker", "Git", "IIS", "CI/CD Pipelines" }
                }
            };
        }

        private List<string> GetCertifications()
        {
            return new List<string>
            {
                "AWS Certified Solutions Architect – Associate",
                "Sitecore 10 .NET Developer Certification",
                "Professional Semicolon Restorer (Self-Certified)"
            };
        }
    }
}
