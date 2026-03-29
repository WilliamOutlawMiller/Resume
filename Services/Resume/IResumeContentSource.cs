using WilliamMillerSite.Models.Resume;

namespace WilliamMillerSite.Services.Resume;

public interface IResumeContentSource
{
    ResumeContentRoot GetContent();
}
