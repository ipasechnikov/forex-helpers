using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ForexHelpers.Tests.Services
{
	internal class CurrencyInterestRatesRefreshWorkerTest
	{
		private CurrencyInterestRatesRefreshWorker _worker = null!;
		private ICurrencyInterestRatesService _service = null!;

		[SetUp]
		public void Setup()
		{
			ServiceCollection services = new ServiceCollection();
			services.AddSingleton<ICurrencyInterestRatesService, EarnForexCurrencyInterestRatesService>();

			// We use AddHostedService to register workers in the app's DI container.
			// But to improve tests quality and trigger worker manually, AddSingleton is called instead.
			services.AddSingleton<CurrencyInterestRatesRefreshWorker>();

			ServiceProvider serviceProvider = services.BuildServiceProvider();
			_worker = serviceProvider.GetRequiredService<CurrencyInterestRatesRefreshWorker>();
			_service = serviceProvider.GetRequiredService<ICurrencyInterestRatesService>();
		}

		[Test]
		public async Task RefreshWorker_Gets_CurrencyInterestRates_InBackground()
		{
			await _worker.StartAsync(CancellationToken.None);
			IEnumerable<CurrencyInterestRate> currencyInterestRates = await _service.GetCurrencyInterestRates();
			Assert.IsNotEmpty(currencyInterestRates);
		}
	}
}