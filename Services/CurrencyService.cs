using System.Globalization;
using System.Xml.Linq;
using Demo.Interfaces;
using Demo.Models;

namespace Demo.Services
{
    public class CurrencyService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        public CurrencyService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Currency>> GetAllExchangeRates()
        {
            var url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var xml = await response.Content.ReadAsStringAsync();

            return ParseXml(xml);
        }

        public List<Currency> ParseXml(string xmlString)
        {
            CultureInfo culture = new CultureInfo("tr-TR"); // Türkçe kültür bilgileri
            var currencyRates = new List<Currency>();
            var document = XDocument.Parse(xmlString);
            foreach (var currencyNode in document.Descendants("Currency"))
            {
                var currencyRate = new Currency
                {
                    CurrencyCode = currencyNode.Element("Isim")?.Value,
                    CurrencyName = currencyNode.Element("CurrencyName")?.Value
                };
                if (!String.IsNullOrEmpty(currencyNode.Element("ForexBuying")?.Value))
                {
                    currencyRate.ForexBuying = decimal.Parse(currencyNode.Element("ForexBuying").Value.Replace(",", "."));
                }
                if (!String.IsNullOrEmpty(currencyNode.Element("ForexSelling")?.Value))
                {
                    currencyRate.ForexSelling = decimal.Parse(currencyNode.Element("ForexSelling").Value.Replace(",", "."));
                }
                if (!String.IsNullOrEmpty(currencyNode.Element("BanknoteBuying")?.Value))
                {
                    currencyRate.BanknoteBuying = decimal.Parse(currencyNode.Element("BanknoteBuying").Value.Replace(",", "."));
                }
                if (!String.IsNullOrEmpty(currencyNode.Element("BanknoteSelling")?.Value))
                {
                    currencyRate.BanknoteSelling = decimal.Parse(currencyNode.Element("BanknoteSelling").Value.Replace(",", "."));
                }
                if (!String.IsNullOrEmpty(currencyNode.Element("CrossRateUSD")?.Value))
                {
                    currencyRate.CrossRateUSD = decimal.Parse(currencyNode.Element("CrossRateUSD").Value.Replace(",", "."));
                }
                if (!String.IsNullOrEmpty(currencyNode.Element("CrossRateOther")?.Value))
                {
                    currencyRate.CrossRateOther = decimal.Parse(currencyNode.Element("CrossRateOther").Value.Replace(",", "."));
                }
                currencyRates.Add(currencyRate);
            }
            return currencyRates;
        }
    }
}