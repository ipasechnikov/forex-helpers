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

		[Test]
		public async Task GetCurrencyInterestRate_Returns_ExpectedCurrency()
		{
			string expectedCurrencyCode = "USD";
			CurrencyInterestRate currencyInterestRate = await _service.GetCurrencyInterestRate(expectedCurrencyCode);
			Assert.That(currencyInterestRate.CurrencyCode, Is.EqualTo(expectedCurrencyCode));
		}

		[Test]
		public void GetCurrencyInterestRate_ThrowsException_IfNoCurrencyFound()
		{
			Assert.ThrowsAsync<KeyNotFoundException>(
				async () => await _service.GetCurrencyInterestRate("ABC")
			);
		}
	}
}
