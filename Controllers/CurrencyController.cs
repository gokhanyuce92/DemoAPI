using Microsoft.AspNetCore.Mvc;
using Demo.Interfaces;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var result = await _currencyService.GetAllExchangeRatesAsync();
            return Ok(result);
        }
    }
}