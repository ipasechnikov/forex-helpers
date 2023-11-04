using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ForexHelpers.Tests.Services
{
	public class EarnForexCurrencyInterestRatesServiceTest
	{
		private EarnForexCurrencyInterestRatesService _service = null!;

		[SetUp]
		public void Setup()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddSingleton<EarnForexCurrencyInterestRatesService>();

			ServiceProvider serviceProvider = services.BuildServiceProvider();
			_service = serviceProvider.GetRequiredService<EarnForexCurrencyInterestRatesService>();
		}

		[Test]
		public async Task InterestRatesTable_ParsesCorrectly()
		{
			IEnumerable<CurrencyInterestRate> currencyInterestRates = await _service.GetCurrencyInterestRates();
			Assert.IsNotEmpty(currencyInterestRates);

			// Run a simple check for USD
			// No need for any additional checks as they are built into service
			CurrencyInterestRate currencyInterestRate = currencyInterestRates.First(x => x.CountryCode == "US");
			Assert.That(currencyInterestRate.CurrencyCode.Length, Is.EqualTo(3));
		}
	}
}
