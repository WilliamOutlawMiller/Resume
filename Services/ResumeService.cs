using Markdig;
using System.Text;
using System.Text.RegularExpressions;

namespace WilliamMillerSite.Services;

public class ResumeService
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<ResumeService> _logger;
    private string? _cachedResumeHtml;
    private string? _cachedResumeMarkdown;

    public ResumeService(IWebHostEnvironment environment, ILogger<ResumeService> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public string GetResumeHtml()
    {
        if (_cachedResumeHtml != null)
            return _cachedResumeHtml;

        var markdown = GetResumeMarkdown();
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        _cachedResumeHtml = Markdown.ToHtml(markdown, pipeline);
        return _cachedResumeHtml;
    }

    public string GetResumeMarkdown()
    {
        if (_cachedResumeMarkdown != null)
            return _cachedResumeMarkdown;

        var resumePath = Path.Combine(_environment.ContentRootPath, "resume.md");
        if (!File.Exists(resumePath))
        {
            _logger.LogWarning("Resume file not found at {Path}", resumePath);
            return string.Empty;
        }

        _cachedResumeMarkdown = File.ReadAllText(resumePath, Encoding.UTF8);
        return _cachedResumeMarkdown;
    }

    public byte[] GetResumeMarkdownBytes()
    {
        var markdown = GetResumeMarkdown();
        return Encoding.UTF8.GetBytes(markdown);
    }

    public ResumeData ParseResume()
    {
        var markdown = GetResumeMarkdown();
        var lines = markdown.Split('\n');
        
        var resume = new ResumeData();
        var currentSection = "";
        var currentExperience = (ExperienceItem?)null;
        var currentProject = (ProjectItem?)null;
        var currentBullets = new List<string>();
        var inExperienceSection = false;
        var inProjectsSection = false;
        var inSummarySection = false;

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.Trim();
            
            if (string.IsNullOrWhiteSpace(trimmed))
                continue;

            if (trimmed.StartsWith("# "))
            {
                var title = trimmed.Substring(2).Trim();
                if (title == "WILLIAM MILLER")
                {
                    continue;
                }
            }
            else if (trimmed.StartsWith("## "))
            {
                var sectionTitle = trimmed.Substring(3).Trim();
                
                if (currentExperience != null && inExperienceSection)
                {
                    if (currentBullets.Any())
                    {
                        currentExperience.BulletPoints = currentBullets.ToList();
                        currentBullets.Clear();
                    }
                    resume.Experiences.Add(currentExperience);
                }
                
                if (currentProject != null && inProjectsSection)
                {
                    if (currentBullets.Any())
                    {
                        currentProject.BulletPoints = currentBullets.ToList();
                        currentBullets.Clear();
                    }
                    resume.Projects.Add(currentProject);
                }
                
                currentSection = sectionTitle;
                inExperienceSection = sectionTitle == "PROFESSIONAL EXPERIENCE";
                inProjectsSection = sectionTitle == "PROJECTS";
                inSummarySection = sectionTitle == "PROFESSIONAL SUMMARY";
                
                currentExperience = null;
                currentProject = null;
                currentBullets.Clear();
            }
            else if (trimmed.StartsWith("### ") && inExperienceSection)
            {
                if (currentExperience != null && currentBullets.Any())
                {
                    currentExperience.BulletPoints = currentBullets.ToList();
                    currentBullets.Clear();
                    resume.Experiences.Add(currentExperience);
                }
                
                var company = trimmed.Substring(4).Trim();
                currentExperience = new ExperienceItem { Company = company };
            }
            else if (trimmed.StartsWith("### ") && inProjectsSection)
            {
                if (currentProject != null && currentBullets.Any())
                {
                    currentProject.BulletPoints = currentBullets.ToList();
                    currentBullets.Clear();
                    resume.Projects.Add(currentProject);
                }
                
                var projectTitle = trimmed.Substring(4).Trim();
                currentProject = new ProjectItem { Title = projectTitle };
            }
            else if (trimmed.StartsWith("#### "))
            {
                continue;
            }
            else if (trimmed.StartsWith("**") && trimmed.Contains("|") && currentExperience != null)
            {
                var roleLine = trimmed.Replace("**", "").Trim();
                var parts = roleLine.Split('|', StringSplitOptions.TrimEntries);
                
                if (parts.Length >= 1)
                {
                    currentExperience.Role = parts[0];
                }
                if (parts.Length >= 2)
                {
                    currentExperience.Location = parts[1];
                }
                if (parts.Length >= 3)
                {
                    currentExperience.Duration = parts[2];
                }
            }
            else if (trimmed.StartsWith("**") && trimmed.Contains("|") && currentProject != null)
            {
                var projectMeta = trimmed.Replace("**", "").Trim();
                var parts = projectMeta.Split('|', StringSplitOptions.TrimEntries);
                
                if (parts.Length >= 1)
                {
                    currentProject.Type = parts[0];
                }
                if (parts.Length >= 2)
                {
                    currentProject.Duration = parts[1];
                }
            }
            else if (trimmed.StartsWith("- "))
            {
                var bullet = trimmed.Substring(2).Trim();
                if (currentExperience != null)
                {
                    currentBullets.Add(bullet);
                }
                else if (currentProject != null)
                {
                    currentBullets.Add(bullet);
                }
            }
            else if (trimmed == "---")
            {
                continue;
            }
            else if (!string.IsNullOrWhiteSpace(trimmed) && inSummarySection)
            {
                if (string.IsNullOrEmpty(resume.Summary))
                {
                    resume.Summary = trimmed;
                }
                else
                {
                    resume.Summary += " " + trimmed;
                }
            }
        }

        if (currentExperience != null)
        {
            if (currentBullets.Any())
            {
                currentExperience.BulletPoints = currentBullets.ToList();
            }
            resume.Experiences.Add(currentExperience);
        }

        if (currentProject != null)
        {
            if (currentBullets.Any())
            {
                currentProject.BulletPoints = currentBullets.ToList();
            }
            resume.Projects.Add(currentProject);
        }

        return resume;
    }
}

public class ResumeData
{
    public string Summary { get; set; } = "";
    public List<ExperienceItem> Experiences { get; set; } = new();
    public List<ProjectItem> Projects { get; set; } = new();
}

public class ExperienceItem
{
    public string Company { get; set; } = "";
    public string Role { get; set; } = "";
    public string Location { get; set; } = "";
    public string Duration { get; set; } = "";
    public List<string> BulletPoints { get; set; } = new();
}

public class ProjectItem
{
    public string Title { get; set; } = "";
    public string Type { get; set; } = "";
    public string Duration { get; set; } = "";
    public List<string> BulletPoints { get; set; } = new();
}
