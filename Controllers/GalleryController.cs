using Microsoft.AspNetCore.Mvc;
using GaoChongPortfolio.Models;

namespace GaoChongPortfolio.Controllers
{
    public class GalleryController : Controller
    {
        public IActionResult Index(string category = "All")
        {
            var projects = GetProjects();
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

        private List<ProjectItem> GetProjects()
        {
            return new List<ProjectItem>
            {
                new ProjectItem
                {
                    Id = "web-portfolio",
                    Title = "ASP.NET Core MVC Portfolio",
                    Description = "The web application you are currently exploring. Architected with strict C# MVC patterns, validated models, and styled using Tailwind CSS. Features an interactive terminal and an Easter Egg hacker mode.",
                    ImageUrl = "/images/projects/portfolio.png",
                    Category = "Web Projects",
                    TechStack = new List<string> { "C#", "ASP.NET Core MVC", "Razor Views", "Tailwind CSS", "JavaScript" },
                    HumorousStat = "Semicolons deployed: 342. Semicolons missing: 0."
                },
                new ProjectItem
                {
                    Id = "smart-shelf",
                    Title = "Smart Shelf OCR System",
                    Description = "A computer-vision-powered retail shelf tracker. Leverages a Flutter mobile application, Dart interfaces, and a C# backend executing live image recognition and counting workflows via OCR.",
                    ImageUrl = "/images/projects/smartshelf.png",
                    Category = "Web Projects",
                    TechStack = new List<string> { "Flutter", "Dart", "C#", "WebSockets", "SQL Server" },
                    HumorousStat = "Accurately identifies item theft. Fails to identify who ate my lunch from the breakroom fridge."
                },
                new ProjectItem
                {
                    Id = "aws-ocr-pipeline",
                    Title = "AWS Serverless OCR Pipeline",
                    Description = "Cloud infrastructure processing edge-camera frames. Utilizes AWS Cognito for auth, Rekognition API for OCR analysis, Lambda for event routing, and DynamoDB for low-latency JSON data binding.",
                    ImageUrl = "/images/projects/cloud_infra.png",
                    Category = "Cloud Infrastructure",
                    TechStack = new List<string> { "AWS Lambda", "Rekognition", "DynamoDB", "Cognito", "Terraform" },
                    HumorousStat = "Monthly cost: $0.12. Monthly coffee budget: $120.00."
                },
                new ProjectItem
                {
                    Id = "custom-pc-build",
                    Title = "Liquid-Cooled Dev Rig",
                    Description = "A custom workstation built for compiler speed and low noise levels. Assembled with an MSI motherboard, Segotep power supply, liquid-loop block, and precise cable management.",
                    ImageUrl = "/images/projects/pcbuild.png",
                    Category = "Hardware Builds",
                    TechStack = new List<string> { "MSI Motherboard", "Segotep PSU", "Liquid Cooling", "RGB Tuning" },
                    HumorousStat = "CPU pins bent: exactly 0. Compilation speeds: faster than compiling excuses."
                },
                new ProjectItem
                {
                    Id = "automotive-ecu-tinker",
                    Title = "ECU Telemetry Logger",
                    Description = "An OBD-II diagnostic logging script connecting a vehicle's Engine Control Unit (ECU) to a dashboard app, tracking coolant temperatures, exhaust ratios, and boost pressure curves.",
                    ImageUrl = "/images/projects/car_tinker.png",
                    Category = "Automotive Tinkering",
                    TechStack = new List<string> { "Python", "OBD-II protocol", "Raspberry Pi", "SQLite" },
                    HumorousStat = "Decoded 14 check engine lights. Turned out the gas cap was just loose 13 times."
                }
            };
        }
    }
}
