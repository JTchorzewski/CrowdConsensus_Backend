using Application.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FinancialDataService : IFinancialDataService
    {
        private readonly IFinancialRepository _repository;

        public FinancialDataService(IFinancialRepository repository)
        {
            _repository = repository;
        }

        public async Task<FinancialData> ScrapeAndSaveDataAsync(string id, string companyName)
        {
            var scrapedData = await FinancialDataScraper.ScrapeNetProfitAsync(id, companyName);
            await _repository.AddAsync(scrapedData);
            return scrapedData;
        }
    }
}
