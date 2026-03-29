using System.Text;
using Markdig;
using WilliamMillerSite.Models.Resume;
using WilliamMillerSite.Services.Resume;

namespace WilliamMillerSite.Services;

public sealed class ResumeService
{
    private readonly IResumeContentSource _contentSource;
    private static readonly MarkdownPipeline MarkdownPipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

    public ResumeService(IResumeContentSource contentSource)
    {
        _contentSource = contentSource;
    }

    public ResumeData ParseResume()
    {
        var root = _contentSource.GetContent();
        return MapToResumeData(root);
    }

    public string GetResumeMarkdown()
    {
        return ResumeMarkdownComposer.ComposeFullMarkdown(_contentSource.GetContent());
    }

    public string GetResumeHtml()
    {
        var markdown = GetResumeMarkdown();
        return Markdown.ToHtml(markdown, MarkdownPipeline);
    }

    public string GetSkillsHtml()
    {
        var skillsMd = _contentSource.GetContent().SkillsMarkdown.Trim();
        if (string.IsNullOrEmpty(skillsMd)) return string.Empty;
        return Markdown.ToHtml(skillsMd, MarkdownPipeline).Trim();
    }

    public string RenderProfileContactHtml()
    {
        var md = _contentSource.GetContent().Profile.ContactMarkdown.Trim();
        if (string.IsNullOrEmpty(md)) return string.Empty;
        return Markdown.ToHtml(md, MarkdownPipeline).Trim();
    }

    public byte[] GetResumeMarkdownBytes()
    {
        return Encoding.UTF8.GetBytes(GetResumeMarkdown());
    }

    public string GetExperienceAnchor(ExperienceItem experience)
    {
        if (experience == null) return string.Empty;
        if (!string.IsNullOrWhiteSpace(experience.AnchorId)) return experience.AnchorId.Trim();
        return ResumeSlug.FromText(experience.Company);
    }

    public string GetProjectElementId(ProjectItem project)
    {
        if (project == null) return "project";
        var slug = string.IsNullOrWhiteSpace(project.AnchorId)
            ? ResumeSlug.FromText(project.Title)
            : project.AnchorId.Trim();
        return $"project-{slug}";
    }

    private static ResumeData MapToResumeData(ResumeContentRoot root)
    {
        return new ResumeData
        {
            Profile = root.Profile ?? new ResumeProfile(),
            Summary = root.Summary ?? "",
            SkillsMarkdown = root.SkillsMarkdown ?? "",
            Experiences = root.Experiences ?? new List<ExperienceItem>(),
            Projects = root.Projects ?? new List<ProjectItem>()
        };
    }
}
