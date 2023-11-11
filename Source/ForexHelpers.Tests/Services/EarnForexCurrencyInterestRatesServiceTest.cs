using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ForexHelpers.Tests.Services
{
	internal class EarnForexCurrencyInterestRatesServiceTest
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
		public async Task GetCurrencyInterestRates_Should_ParseHtmlInterestRatesTableCorrectly()
		{
			IEnumerable<CurrencyInterestRate> currencyInterestRates = await _service.GetCurrencyInterestRates();
			Assert.IsNotEmpty(currencyInterestRates);

			// Run a simple check for USD
			// No need for any additional checks as they are built into service
			CurrencyInterestRate currencyInterestRate = currencyInterestRates.First(x => x.CountryCode == "US");
			Assert.That(currencyInterestRate.CurrencyCode.Length, Is.EqualTo(3));
		}

		[Test]
		public async Task GetCurrencyInterestRate_Should_ReturnRequestedCurrencyIfCurrencyIsPresent()
		{
			string requestedCurrencyCode = "USD";
			CurrencyInterestRate? currencyInterestRate = await _service.GetCurrencyInterestRate(requestedCurrencyCode);
			Assert.IsNotNull(currencyInterestRate);
			Assert.That(currencyInterestRate.CurrencyCode, Is.EqualTo(requestedCurrencyCode));
		}

		[Test]
		public async Task GetCurrencyInterestRate_Should_ReturnNullIfRequestCurrencyIsMissing()
		{
			string requestedCurrencyCode = "ABC";
			CurrencyInterestRate? currencyInterestRate = await _service.GetCurrencyInterestRate(requestedCurrencyCode);
			Assert.IsNull(currencyInterestRate);
		}
	}
}