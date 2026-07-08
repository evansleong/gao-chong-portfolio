using System.Collections.Generic;

namespace GaoChongPortfolio.Models
{
    public class PortfolioData
    {
        public BioInfo Bio { get; set; } = new();
        public List<ExperienceItem> Experiences { get; set; } = new();
        public List<ProjectItem> Projects { get; set; } = new();
    }

    public class BioInfo
    {
        public string HeroTitle { get; set; } = "Console.WriteLine(\"Hello, World!\");";
        public string HeroSubtitle { get; set; } = "Software Engineer specializing in C#, ASP.NET, Sitecore, Azure, AWS, and full-stack development. I enjoy solving complex production issues, building scalable applications, and turning cryptic logs into meaningful solutions.";
        public string BioText1 { get; set; } = "I am a software engineer with experience in enterprise application support, cloud technologies, and full-stack development. My work spans C#, ASP.NET MVC, Microsoft Azure, Sitecore, AWS, and modern web technologies, with a strong focus on writing maintainable, scalable, and production-ready software.";
        public string BioText2 { get; set; } = "From investigating Sitecore production issues and analyzing Azure logs with Kusto Query Language (KQL) to developing AI-powered IoT solutions and cloud-integrated applications, I enjoy tackling challenging technical problems through structured debugging, clean architecture, and continuous learning.";
        public string BioHumorousNote { get; set; } = "Humorous Note: Professionally trained to read Sitecore logs for hours, debug recursive stack traces, and somehow keep production deployments drama-free.";
        public string AvatarUrl { get; set; } = string.Empty;
        
        // Stats
        public string CoffeeIntake { get; set; } = "1.8 Liters/Day";
        public string CpuPinsBent { get; set; } = "0 Pins (Optimal)";
        public string PreferredEditor { get; set; } = "VS Code / Visual Studio";
        public string OsHost { get; set; } = "Windows";
        public string ProductionHotfixes { get; set; } = "Logs First, Deploy Second";
        public string HumorLevel { get; set; } = "try { Code(); } catch { Google(); }";
    }
}
