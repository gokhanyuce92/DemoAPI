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
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        /// <summary>
        /// UserPolicy yetkisine sahip kullanıcılar döviz kurlarını çekebilir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var currencies = await _currencyService.GetAllExchangeRatesAsync();

            return Ok(currencies);
        }
    }
}