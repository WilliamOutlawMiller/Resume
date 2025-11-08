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
        if (_cachedResumeHtml != null) return _cachedResumeHtml;

        var markdown = GetResumeMarkdown();
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        var html = Markdown.ToHtml(markdown, pipeline);
        
        _cachedResumeHtml = AddExperienceSectionIds(html);
        return _cachedResumeHtml;
    }

    public string GetSkillsHtml()
    {
        var markdown = GetResumeMarkdown();
        var lines = markdown.Split('\n');
        var skillsLines = new List<string>();
        var inSkillsSection = false;

        foreach (var line in lines)
        {
            var trimmed = line.Trim();
            
            if (trimmed.StartsWith("## ") && trimmed.Contains("TECHNICAL SKILLS", StringComparison.OrdinalIgnoreCase))
            {
                inSkillsSection = true;
                continue;
            }
            
            if (inSkillsSection)
            {
                if (trimmed.StartsWith("## ") || trimmed == "---") break;
                if (!string.IsNullOrWhiteSpace(trimmed)) skillsLines.Add(line);
            }
        }

        if (skillsLines.Count == 0) return string.Empty;

        var skillsMarkdown = string.Join("\n", skillsLines);
        var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
        return Markdown.ToHtml(skillsMarkdown, pipeline).Trim();
    }

    private string AddExperienceSectionIds(string html)
    {
        var resumeData = ParseResume();
        var result = html;
        
        foreach (var exp in resumeData.Experiences)
        {
            var companySlug = GenerateSlug(exp.Company);
            var escapedCompany = Regex.Escape(exp.Company);
            var h3Pattern = $@"<h3>\s*({escapedCompany})\s*</h3>";
            var replacement = $@"<h3 id=""{companySlug}"">$1</h3>";
            
            if (Regex.IsMatch(result, h3Pattern, RegexOptions.IgnoreCase))
            {
                result = Regex.Replace(result, h3Pattern, replacement, RegexOptions.IgnoreCase);
            }
            else
            {
                var firstWord = exp.Company.Split(new[] { ' ', '–', '-' }, StringSplitOptions.RemoveEmptyEntries)[0];
                var flexiblePattern = $@"<h3>([^<]*{Regex.Escape(firstWord)}[^<]*)</h3>";
                result = Regex.Replace(result, flexiblePattern, match =>
                {
                    var content = match.Groups[1].Value.Trim();
                    if (content.Contains(firstWord, StringComparison.OrdinalIgnoreCase))
                        return $@"<h3 id=""{companySlug}"">{content}</h3>";
                    return match.Value;
                }, RegexOptions.IgnoreCase);
            }
        }
        
        foreach (var project in resumeData.Projects)
        {
            var projectSlug = $"project-{project.Title.ToLower().Replace(" ", "-")}";
            var escapedTitle = Regex.Escape(project.Title);
            var h3Pattern = $@"<h3>\s*({escapedTitle})\s*</h3>";
            var replacement = $@"<h3 id=""{projectSlug}"">$1</h3>";
            
            if (Regex.IsMatch(result, h3Pattern, RegexOptions.IgnoreCase))
            {
                result = Regex.Replace(result, h3Pattern, replacement, RegexOptions.IgnoreCase);
            }
        }
        
        return result;
    }

    public string GetCompanySlug(string companyName) => GenerateSlug(companyName);

    private string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;
        
        var slug = text.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        return slug.Trim('-');
    }

    public string GetResumeMarkdown()
    {
        if (_cachedResumeMarkdown != null) return _cachedResumeMarkdown;

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
        return Encoding.UTF8.GetBytes(GetResumeMarkdown());
    }

    public ResumeData ParseResume()
    {
        var markdown = GetResumeMarkdown();
        var lines = markdown.Split('\n');
        
        var resume = new ResumeData();
        var currentExperience = (ExperienceItem?)null;
        var currentProject = (ProjectItem?)null;
        var currentParagraphs = new List<string>();
        var currentParagraph = new List<string>();
        var inExperienceSection = false;
        var inProjectsSection = false;
        var inSummarySection = false;

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i];
            var trimmed = line.Trim();
            
            if (string.IsNullOrWhiteSpace(trimmed))
            {
                if (currentParagraph.Any() && (currentExperience != null || currentProject != null))
                {
                    var paragraphText = string.Join(" ", currentParagraph).Trim();
                    if (!string.IsNullOrWhiteSpace(paragraphText))
                        currentParagraphs.Add(paragraphText);
                    currentParagraph.Clear();
                }
                continue;
            }

            if (trimmed.StartsWith("# ")) continue;
            
            if (trimmed.StartsWith("## "))
            {
                var sectionTitle = trimmed.Substring(3).Trim();
                
                if (currentExperience != null && inExperienceSection)
                {
                    if (currentParagraphs.Any()) currentExperience.BulletPoints = currentParagraphs.ToList();
                    resume.Experiences.Add(currentExperience);
                }
                
                if (currentProject != null && inProjectsSection)
                {
                    if (currentParagraphs.Any()) currentProject.BulletPoints = currentParagraphs.ToList();
                    ExtractTechnologies(currentProject);
                    resume.Projects.Add(currentProject);
                }
                
                inExperienceSection = sectionTitle == "PROFESSIONAL EXPERIENCE";
                inProjectsSection = sectionTitle == "PROJECTS";
                inSummarySection = sectionTitle == "PROFESSIONAL SUMMARY";
                
                currentExperience = null;
                currentProject = null;
                currentParagraphs.Clear();
                currentParagraph.Clear();
            }
            else if (trimmed.StartsWith("### ") && inExperienceSection)
            {
                if (currentExperience != null && currentParagraphs.Any())
                {
                    currentExperience.BulletPoints = currentParagraphs.ToList();
                    resume.Experiences.Add(currentExperience);
                }
                
                currentExperience = new ExperienceItem { Company = trimmed.Substring(4).Trim() };
                currentParagraphs.Clear();
                currentParagraph.Clear();
            }
            else if (trimmed.StartsWith("### ") && inProjectsSection)
            {
                if (currentProject != null && currentParagraphs.Any())
                {
                    currentProject.BulletPoints = currentParagraphs.ToList();
                    ExtractTechnologies(currentProject);
                    resume.Projects.Add(currentProject);
                }
                
                currentProject = new ProjectItem { Title = trimmed.Substring(4).Trim() };
                currentParagraphs.Clear();
                currentParagraph.Clear();
            }
            else if (trimmed.StartsWith("#### "))
            {
                if (currentParagraph.Any() && (currentExperience != null || currentProject != null))
                {
                    var paragraphText = string.Join(" ", currentParagraph).Trim();
                    if (!string.IsNullOrWhiteSpace(paragraphText))
                        currentParagraphs.Add(paragraphText);
                    currentParagraph.Clear();
                }
                continue;
            }
            else if (trimmed.StartsWith("**") && trimmed.Contains("|") && currentExperience != null)
            {
                if (currentParagraph.Any())
                {
                    var paragraphText = string.Join(" ", currentParagraph).Trim();
                    if (!string.IsNullOrWhiteSpace(paragraphText))
                        currentParagraphs.Add(paragraphText);
                    currentParagraph.Clear();
                }
                
                var roleLine = trimmed.Replace("**", "").Trim();
                var parts = roleLine.Split('|', StringSplitOptions.TrimEntries);
                
                if (parts.Length >= 1) currentExperience.Role = parts[0];
                if (parts.Length >= 2) currentExperience.Location = parts[1];
                if (parts.Length >= 3) currentExperience.Duration = parts[2];
            }
            else if (trimmed.StartsWith("**") && trimmed.Contains("|") && currentProject != null)
            {
                if (currentParagraph.Any())
                {
                    var paragraphText = string.Join(" ", currentParagraph).Trim();
                    if (!string.IsNullOrWhiteSpace(paragraphText))
                        currentParagraphs.Add(paragraphText);
                    currentParagraph.Clear();
                }
                
                var projectMeta = trimmed.Replace("**", "").Trim();
                var parts = projectMeta.Split('|', StringSplitOptions.TrimEntries);
                
                if (parts.Length >= 1) currentProject.Type = parts[0];
                if (parts.Length >= 2) currentProject.Duration = parts[1];
            }
            else if (trimmed == "---") continue;
            else if (!string.IsNullOrWhiteSpace(trimmed) && inSummarySection)
            {
                resume.Summary = string.IsNullOrEmpty(resume.Summary) ? trimmed : resume.Summary + " " + trimmed;
            }
            else if ((currentExperience != null || currentProject != null) && 
                     !trimmed.StartsWith("#") && 
                     !trimmed.StartsWith("**") &&
                     !trimmed.StartsWith("["))
            {
                currentParagraph.Add(trimmed);
            }
        }

        if (currentParagraph.Any() && (currentExperience != null || currentProject != null))
        {
            var paragraphText = string.Join(" ", currentParagraph).Trim();
            if (!string.IsNullOrWhiteSpace(paragraphText))
                currentParagraphs.Add(paragraphText);
        }

        if (currentExperience != null)
        {
            if (currentParagraphs.Any()) currentExperience.BulletPoints = currentParagraphs.ToList();
            resume.Experiences.Add(currentExperience);
        }

        if (currentProject != null)
        {
            if (currentParagraphs.Any()) currentProject.BulletPoints = currentParagraphs.ToList();
            ExtractTechnologies(currentProject);
            resume.Projects.Add(currentProject);
        }

        return resume;
    }
    
    private void ExtractTechnologies(ProjectItem project)
    {
        var allText = string.Join(" ", project.BulletPoints).ToLower();
        var technologies = new HashSet<string>();
        
        var techKeywords = new Dictionary<string, string[]>
        {
            { "Python", new[] { "python" } },
            { "C#", new[] { "c#", "c sharp", ".net core", ".net 8", ".net framework" } },
            { "Angular", new[] { "angular" } },
            { "Entity Framework", new[] { "entity framework" } },
            { "SQLite", new[] { "sqlite" } },
            { "SignalR", new[] { "signalr" } },
            { "NGINX", new[] { "nginx" } },
            { "GitHub Actions", new[] { "github actions" } },
            { "ASP.NET Core", new[] { "asp.net core", "razor pages" } },
            { "Razor Pages", new[] { "razor pages" } },
            { "Java", new[] { "java" } },
            { "Docker", new[] { "docker", "dockerfile" } },
            { "Kubernetes", new[] { "kubernetes" } },
            { "Airflow", new[] { "airflow" } },
            { "Azure DevOps", new[] { "azure devops" } },
            { "Flask", new[] { "flask" } },
            { "FastAPI", new[] { "fastapi" } },
            { "Uvicorn", new[] { "uvicorn" } },
            { "SQL Server", new[] { "sql server" } },
            { "Oracle", new[] { "oracle" } },
            { "WCF", new[] { "wcf" } }
        };
        
        foreach (var tech in techKeywords)
        {
            if (tech.Value.Any(keyword => allText.Contains(keyword)))
                technologies.Add(tech.Key);
        }
        
        var projectTitleLower = project.Title.ToLower();
        
        if (projectTitleLower.Contains("muni optimizer"))
        {
            technologies.Add("Oracle");
            technologies.Add("MSSQL");
        }
        else if (projectTitleLower.Contains("securitized assets universe data pipeline"))
        {
            technologies.Add("Excel");
            technologies.Add("Docker");
        }
        else if (projectTitleLower.Contains("rate lock vendor file parser"))
        {
            technologies.Remove("Docker");
            technologies.Add("Python");
            technologies.Add("AI");
            technologies.Add("Docker");
        }
        
        project.Technologies = technologies.OrderBy(t => t).ToList();
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
    public List<string> Technologies { get; set; } = new();
}
