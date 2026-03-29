using System.Text.RegularExpressions;

namespace WilliamMillerSite.Services.Resume;

public static class ResumeSlug
{
    public static string FromText(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return string.Empty;

        var slug = text.ToLowerInvariant();
        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-");
        slug = Regex.Replace(slug, @"-+", "-");
        return slug.Trim('-');
    }
}
