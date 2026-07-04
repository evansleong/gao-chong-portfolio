namespace GaoChongPortfolio.Models
{
    public class ProjectViewModel
    {
        public List<ProjectItem> Projects { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string ActiveCategory { get; set; } = "All";
    }
}
