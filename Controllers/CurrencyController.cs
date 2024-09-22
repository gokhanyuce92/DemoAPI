using Microsoft.AspNetCore.Mvc;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Demo.Services;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        private readonly ISearchService<Currency> _searchService;
        public CurrencyController(ICurrencyService currencyService, ISearchService<Currency> searchService)
        {
            _currencyService = currencyService;
            _searchService = searchService;
        }

        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> Get()
        {
            var currencies = await _currencyService.GetAllExchangeRatesAsync();

            foreach (var currency in currencies)
            {
                await _searchService.AddDocumentAsync("currency-index", currency);
            }
            return Ok(currencies);
        }
    }
}