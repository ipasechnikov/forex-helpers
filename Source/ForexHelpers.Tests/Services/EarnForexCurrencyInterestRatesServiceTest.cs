using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;

using Microsoft.Extensions.Configuration;
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
			await _service.GetCurrencyInterestRates();
			Assert.IsNotEmpty(_service.CurrencyInterestRates);

			CurrencyInterestRate currencyInterestRate = _service.CurrencyInterestRates.First();
			Assert.Equals(currencyInterestRate.CurrencyCode.Length, 2);
		}
	}
}
