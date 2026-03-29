using System.Text;
using WilliamMillerSite.Models.Resume;

namespace WilliamMillerSite.Services.Resume;

public static class ResumeMarkdownComposer
{
    public static string ComposeFullMarkdown(ResumeContentRoot root)
    {
        var sb = new StringBuilder();

        sb.Append("# ").AppendLine(EscapeForHeading(root.Profile.Heading));
        sb.AppendLine();
        sb.AppendLine(root.Profile.ContactMarkdown.Trim());
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## PROFESSIONAL SUMMARY");
        sb.AppendLine();
        sb.AppendLine(root.Summary.Trim());
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## TECHNICAL SKILLS");
        sb.AppendLine();
        sb.AppendLine(root.SkillsMarkdown.Trim());
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine("## PROFESSIONAL EXPERIENCE");
        sb.AppendLine();

        foreach (var exp in root.Experiences)
        {
            var anchor = string.IsNullOrWhiteSpace(exp.AnchorId) ? ResumeSlug.FromText(exp.Company) : exp.AnchorId!.Trim();
            sb.Append("<a id=\"").Append(HtmlEscapeAttr(anchor)).AppendLine("\"></a>");
            sb.AppendLine();
            sb.Append("### ").AppendLine(exp.Company.Trim());
            sb.AppendLine();
            sb.Append("**").Append(exp.Role.Trim()).Append(" | ").Append(exp.Location.Trim()).Append(" | ").Append(exp.Duration.Trim()).AppendLine("**");
            sb.AppendLine();
            foreach (var bullet in exp.BulletPoints)
            {
                if (string.IsNullOrWhiteSpace(bullet)) continue;
                sb.AppendLine(bullet.Trim());
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine();
        }

        sb.AppendLine("## PROJECTS");
        sb.AppendLine();

        foreach (var project in root.Projects)
        {
            var anchor = string.IsNullOrWhiteSpace(project.AnchorId) ? ResumeSlug.FromText(project.Title) : project.AnchorId!.Trim();
            sb.Append("<a id=\"project-").Append(HtmlEscapeAttr(anchor)).AppendLine("\"></a>");
            sb.AppendLine();
            sb.Append("### ").AppendLine(project.Title.Trim());
            sb.AppendLine();
            var meta = string.IsNullOrWhiteSpace(project.Duration)
                ? $"**{project.Type.Trim()}**"
                : $"**{project.Type.Trim()} | {project.Duration.Trim()}**";
            sb.AppendLine(meta);
            sb.AppendLine();
            foreach (var bullet in project.BulletPoints)
            {
                if (string.IsNullOrWhiteSpace(bullet)) continue;
                sb.AppendLine(bullet.Trim());
                sb.AppendLine();
            }

            sb.AppendLine("---");
            sb.AppendLine();
        }

        return sb.ToString().TrimEnd();
    }

    private static string EscapeForHeading(string text) => text.Trim();

    private static string HtmlEscapeAttr(string value)
    {
        return value.Replace("&", "&amp;", StringComparison.Ordinal)
            .Replace("\"", "&quot;", StringComparison.Ordinal)
            .Replace("<", "&lt;", StringComparison.Ordinal);
    }
}
