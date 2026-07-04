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
        public string HeroSubtitle { get; set; } = "I build scalable full-stack applications, untangle chaotic log files, and occasionally build custom PCs without bending the CPU pins.";
        public string BioText1 { get; set; } = "I am a full-stack software engineer with a heavy leaning towards robust, production-grade backend architectures, cloud integrations, and clean code. I believe code should not only compile, but be structured in a way that doesn't trigger migraines for future developers.";
        public string BioText2 { get; set; } = "Over my years of writing code, I have survived countless recursive error loops, hunted down memory leaks in vendor DLL assemblies, and wrangled distributed cloud pipelines. Whether it's managing database deadlocks or building reactive mobile frontends, my approach is methodical, automated, and tested.";
        public string BioHumorousNote { get; set; } = "Humorous Note: I am certified in reading sitecore logs without falling asleep, and I've successfully deployed code to production on a Friday without causing server explosions.";
        
        // Stats
        public string CoffeeIntake { get; set; } = "1.8 Liters/Day";
        public string CpuPinsBent { get; set; } = "0 Pins (Optimal)";
        public string PreferredEditor { get; set; } = "VS Code / Rider";
        public string OsHost { get; set; } = "Windows/Linux WSL";
        public string ProductionHotfixes { get; set; } = "0 (Tested locally!)";
        public string HumorLevel { get; set; } = "Strictly Catch-Block";
    }
}
