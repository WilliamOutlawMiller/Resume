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
        var resumeData = _resumeService.ParseResume();
        return View(resumeData);
    }

    public IActionResult Resume()
    {
        var resumeHtml = _resumeService.GetResumeHtml();
        ViewBag.ResumeHtml = resumeHtml;
        return View();
    }

    public IActionResult DownloadResume()
    {
        var markdownBytes = _resumeService.GetResumeMarkdownBytes();
        return File(markdownBytes, "text/markdown", "WilliamMiller_Resume.md");
    }

    public IActionResult Error()
    {
        return View();
    }
}

