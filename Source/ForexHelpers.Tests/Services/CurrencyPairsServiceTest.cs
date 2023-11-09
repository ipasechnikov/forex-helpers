using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ForexHelpers.Tests.Services
{
    internal class CurrencyPairsServiceTest
    {
        private CurrencyPairsService _service = null!;

        [SetUp]
        public void Setup()
        {
            ServiceCollection services = new ServiceCollection();
            services.AddSingleton<ICurrencyInterestRatesService, EarnForexCurrencyInterestRatesService>();
            services.AddSingleton<CurrencyPairsService>();

            ServiceProvider serviceProvider = services.BuildServiceProvider();
            _service = serviceProvider.GetRequiredService<CurrencyPairsService>();
        }

        [Test]
        public async Task GetCurrencyPairs_Returns_CurrencyPairs()
        {
            IEnumerable<CurrencyPair> currencyPairs = await _service.GetCurrencyPairs();
            Assert.IsNotEmpty(currencyPairs);

            string expectedBase = "EUR";
            string expectedQuote = "USD";
            CurrencyPair currencyPair = currencyPairs.First(cp => cp.Base == expectedBase && cp.Quote == expectedQuote);
            Assert.That(currencyPair.Base, Is.EqualTo(expectedBase));
            Assert.That(currencyPair.Quote, Is.EqualTo(expectedQuote));
        }
    }
}