using Microsoft.AspNetCore.Mvc;
using WilliamMillerSite.Services;

namespace WilliamMillerSite.Controllers;

public class HomeController : Controller
{
    private readonly ResumeService _resumeService;

    public HomeController(ResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    public IActionResult Index()
    {
        return View(_resumeService.ParseResume());
    }

    public IActionResult Resume()
    {
        ViewBag.ResumeHtml = _resumeService.GetResumeHtml();
        ViewBag.SkillsHtml = _resumeService.GetSkillsHtml();
        ViewBag.ProfileContactHtml = _resumeService.RenderProfileContactHtml();
        ViewBag.ResumeData = _resumeService.ParseResume();
        return View();
    }

    [HttpGet("Resume/Download")]
    public IActionResult DownloadResume()
    {
        var markdownBytes = _resumeService.GetResumeMarkdownBytes();
        return File(markdownBytes, "text/markdown; charset=utf-8", "WilliamMiller_Resume.md");
    }

    public IActionResult Error()
    {
        return View();
    }
}
