using Microsoft.AspNetCore.Mvc;
using GaoChongPortfolio.Models;

namespace GaoChongPortfolio.Controllers
{
    public class ResumeController : Controller
    {
        public IActionResult Index()
        {
            var model = GetResumeData();
            return View(model);
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

            switch (cleanCmd)
            {
                case "help":
                    response = "Available commands:\r\n  help        - Display this menu\r\n  bio         - Print developer profile description\r\n  skills      - Query technical skills array\r\n  experience  - List work experience data\r\n  hire        - Initiate onboarding sequence\r\n  matrix      - Trigger retro overlay\r\n  sudo rm -rf / - Wipe root partition";
                    break;
                case "bio":
                    response = "Gao Chong - Senior Full-Stack Engineer\r\n---------------------------------------\r\nSpecialty: High-throughput backend C# microservices, AWS serverless pipelines, and Sitecore enterprise architecture. Survives recursive code traps, deep-dives into log outputs, and builds systems that don't leak memory.";
                    break;
                case "skills":
                    response = "System Skills Query Results:\r\n  * Languages: C#, Python, JavaScript, Dart, SQL, HTML/CSS\r\n  * Frameworks: ASP.NET Core MVC, React, Flutter, Sitecore (10.3/10.4)\r\n  * Cloud: AWS Lambda, Rekognition, DynamoDB, Cognito, S3\r\n  * Tools: ILSpy, Log Analyzer, Git, Docker, IIS";
                    break;
                case "experience":
                    response = "Work History:\r\n  [1] Full-Stack & Cloud Integration Engineer\r\n      Built Smart Shelf System with Flutter, Dart, C#, and AWS Rekognition/Lambda.\r\n  [2] Enterprise Platform Debugger (Sitecore 10.3/10.4)\r\n      Mitigated production log-flooding, decompiled black-box DLLs using ILSpy.";
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

        private ResumeViewModel GetResumeData()
        {
            return new ResumeViewModel
            {
                Experiences = new List<ExperienceItem>
                {
                    new ExperienceItem
                    {
                        Company = "Smart Retail Solutions Inc.",
                        Role = "Full-Stack & Cloud Integration Engineer",
                        Duration = "2024 - Present",
                        BulletPoints = new List<string>
                        {
                            "Designed and implemented a real-time Smart Shelf System combining Flutter/Dart mobile apps with an ASP.NET Core backend.",
                            "Leveraged AWS Rekognition for item detection and OCR, DynamoDB for serverless storage, and Cognito for authorization.",
                            "Developed high-efficiency serverless endpoints in AWS Lambda to process live image captures under 200ms."
                        },
                        HumorousTakeaway = "No CPU pins were bent in the physical assembly of the edge sensors. Rekognition accurately tells Cheetos apart from Doritos."
                    },
                    new ExperienceItem
                    {
                        Company = "Enterprise Core Diagnostics",
                        Role = "Enterprise Platform Debugger & Architect",
                        Duration = "2022 - 2024",
                        BulletPoints = new List<string>
                        {
                            "Provided L3 technical support and platform diagnostics for enterprise Sitecore 10.3/10.4 configurations.",
                            "Decompiled and debugged proprietary assemblies using ILSpy to identify memory leaks and logic flaws.",
                            "Built customized tools based on Log Analyzer to mitigate production log-flooding, reducing disk writes by 75%."
                        },
                        HumorousTakeaway = "Sitecore logs are like a fantasy novel: long, confusing, and full of unexpected stack traces. ILSpy was my sword."
                    }
                },
                SkillGroups = new List<SkillGroup>
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
                },
                Certifications = new List<string>
                {
                    "AWS Certified Solutions Architect – Associate",
                    "Sitecore 10 .NET Developer Certification",
                    "Professional Semicolon Restorer (Self-Certified)"
                }
            };
        }
    }
}
