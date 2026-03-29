using System.Text.Json.Serialization;

namespace WilliamMillerSite.Models.Resume;

public sealed class ProjectItem
{
    [JsonPropertyName("anchorId")]
    public string? AnchorId { get; set; }

    public string Title { get; set; } = "";

    public string Type { get; set; } = "";

    public string Duration { get; set; } = "";

    public List<string> BulletPoints { get; set; } = new();

    public List<string> Technologies { get; set; } = new();
}
