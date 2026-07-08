using System.IO;
using System.Text.Json;
using GaoChongPortfolio.Models;
using Microsoft.AspNetCore.Hosting;

namespace GaoChongPortfolio.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly string _filePath;
        private readonly object _lock = new();

        public PortfolioService(IWebHostEnvironment webHostEnvironment)
        {
            string dataDir;
            if (Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID") != null)
            {
                // Azure persistent writeable volume
                dataDir = "/home/site/data";
            }
            else
            {
                // Local development fallback
                var rootPath = webHostEnvironment.WebRootPath ?? Path.Combine(webHostEnvironment.ContentRootPath, "wwwroot");
                dataDir = Path.Combine(rootPath, "data");
            }
            
            // Ensure directory exists
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }

            _filePath = Path.Combine(dataDir, "portfolio.json");
        }

        public PortfolioData GetData()
        {
            lock (_lock)
            {
                if (!File.Exists(_filePath))
                {
                    var defaultData = SeedDefaultData();
                    SaveData(defaultData);
                    return defaultData;
                }

                try
                {
                    var json = File.ReadAllText(_filePath);
                    var data = JsonSerializer.Deserialize<PortfolioData>(json);
                    
                    if (data == null)
                    {
                        return SeedDefaultData();
                    }

                    // Enforce non-null properties to prevent NullReferenceExceptions in views
                    if (data.Bio == null) data.Bio = new BioInfo();
                    if (data.Experiences == null) data.Experiences = new List<ExperienceItem>();
                    if (data.Projects == null) data.Projects = new List<ProjectItem>();

                    return data;
                }
                catch
                {
                    // Fallback to defaults if file is corrupted or unreadable
                    return SeedDefaultData();
                }
            }
        }

        public void SaveData(PortfolioData data)
        {
            lock (_lock)
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(data, options);
                File.WriteAllText(_filePath, json);
            }
        }

        private PortfolioData SeedDefaultData()
        {
            return new PortfolioData
            {
                Bio = new BioInfo(),
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
                Projects = new List<ProjectItem>
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
                }
            };
        }
    }
}
