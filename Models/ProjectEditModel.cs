using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GaoChongPortfolio.Models
{
    public class ProjectEditModel
    {
        [Required(ErrorMessage = "Unique ID is required (kebab-case recommended).")]
        [RegularExpression(@"^[a-z0-9\-]+$", ErrorMessage = "ID must be lowercase alphanumeric and dashes only.")]
        [Display(Name = "Project Unique Key (e.g. smart-shelf)")]
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        [Display(Name = "Display Title")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Project Description is required.")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        [Display(Name = "Description (Details)")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Image File Upload (Overrides current URL)")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Fallback Image URL (If no file uploaded)")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Category selection is required.")]
        [Display(Name = "Registry Category")]
        public string Category { get; set; } = "Web Projects"; // e.g. "Web Projects", "Cloud Infrastructure", "Hardware Builds", "Automotive Tinkering"

        [Display(Name = "Tech Stack tags (comma-separated, e.g. C#, AWS, React)")]
        public string TechStackRaw { get; set; } = string.Empty;

        [Display(Name = "Humorous Stat / Diagnostic Note")]
        [StringLength(200, ErrorMessage = "Humorous stat cannot exceed 200 characters.")]
        public string HumorousStat { get; set; } = string.Empty;
    }
}
