using Demo.Models;

namespace Demo.Interfaces
{
    public interface ICurrencyService
    {
        Task<List<Currency>> GetAllExchangeRates();
    }
}