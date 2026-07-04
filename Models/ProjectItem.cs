namespace GaoChongPortfolio.Models
{
    public class ProjectItem
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g. "Web Projects", "Cloud Infrastructure", "Hardware Builds", "Automotive Tinkering"
        public List<string> TechStack { get; set; } = new();
        public string HumorousStat { get; set; } = string.Empty; // Witty dev-focused note
    }
}
