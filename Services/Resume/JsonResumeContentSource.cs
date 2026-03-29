using System.Text.Json;
using WilliamMillerSite.Models.Resume;

namespace WilliamMillerSite.Services.Resume;

public sealed class JsonResumeContentSource : IResumeContentSource
{
    private readonly IWebHostEnvironment _environment;
    private readonly ILogger<JsonResumeContentSource> _logger;
    private readonly object _gate = new();
    private ResumeContentRoot? _cache;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        ReadCommentHandling = JsonCommentHandling.Skip,
        AllowTrailingCommas = true
    };

    public JsonResumeContentSource(IWebHostEnvironment environment, ILogger<JsonResumeContentSource> logger)
    {
        _environment = environment;
        _logger = logger;
    }

    public ResumeContentRoot GetContent()
    {
        if (_environment.IsDevelopment())
            return LoadFromDisk();

        lock (_gate)
        {
            if (_cache != null) return _cache;
            _cache = LoadFromDisk();
            return _cache;
        }
    }

    private ResumeContentRoot LoadFromDisk()
    {
        var path = Path.Combine(_environment.ContentRootPath, "Data", "resume.json");
        if (!File.Exists(path))
        {
            _logger.LogError("Resume configuration missing at {Path}", path);
            return new ResumeContentRoot();
        }

        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<ResumeContentRoot>(json, JsonOptions) ?? new ResumeContentRoot();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to deserialize resume.json");
            return new ResumeContentRoot();
        }
    }
}
