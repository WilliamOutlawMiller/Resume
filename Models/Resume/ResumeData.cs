namespace WilliamMillerSite.Models.Resume;

public sealed class ResumeData
{
    public ResumeProfile Profile { get; set; } = new();

    public string Summary { get; set; } = "";

    public string SkillsMarkdown { get; set; } = "";

    public List<ExperienceItem> Experiences { get; set; } = new();

    public List<ProjectItem> Projects { get; set; } = new();
}
