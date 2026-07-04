namespace GaoChongPortfolio.Models
{
    public class ResumeViewModel
    {
        public List<ExperienceItem> Experiences { get; set; } = new();
        public List<SkillGroup> SkillGroups { get; set; } = new();
        public List<string> Certifications { get; set; } = new();
    }

    public class ExperienceItem
    {
        public string Company { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Duration { get; set; } = string.Empty;
        public List<string> BulletPoints { get; set; } = new();
        public string HumorousTakeaway { get; set; } = string.Empty;
    }

    public class SkillGroup
    {
        public string CategoryName { get; set; } = string.Empty; // e.g. "Languages", "Backend & Clouds", "Tools"
        public List<string> Skills { get; set; } = new();
    }
}
