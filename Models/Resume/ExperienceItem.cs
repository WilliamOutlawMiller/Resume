using System.Text.Json.Serialization;

namespace WilliamMillerSite.Models.Resume;

public sealed class ExperienceItem
{
    [JsonPropertyName("anchorId")]
    public string? AnchorId { get; set; }

    public string Company { get; set; } = "";

    public string Role { get; set; } = "";

    public string Location { get; set; } = "";

    public string Duration { get; set; } = "";

    public List<string> BulletPoints { get; set; } = new();
}
