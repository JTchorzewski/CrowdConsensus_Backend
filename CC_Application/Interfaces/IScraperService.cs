using HtmlAgilityPack;

namespace Application.Interfaces;

public interface IScraperService
{
    public Task ScrapeAndAddCompanyData(string symbol);
}