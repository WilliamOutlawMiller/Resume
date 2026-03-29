using System.Text.Json.Serialization;

namespace WilliamMillerSite.Models.Resume;

public sealed class ResumeContentRoot
{
    public ResumeProfile Profile { get; set; } = new();

    public string Summary { get; set; } = "";

    [JsonPropertyName("skillsMarkdown")]
    public string SkillsMarkdown { get; set; } = "";

    public List<ExperienceItem> Experiences { get; set; } = new();

    public List<ProjectItem> Projects { get; set; } = new();
}
