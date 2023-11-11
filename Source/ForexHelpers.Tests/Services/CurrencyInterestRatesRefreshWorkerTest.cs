using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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

            Mock<ICurrencyInterestRatesService> serviceMock = new Mock<ICurrencyInterestRatesService>();
			serviceMock.Setup(service => service.RefreshCurrencyInterestRates())
				.Callback(() =>
				{
                    serviceMock.Setup(service => service.GetCurrencyInterestRates())
                        .Returns(Task.FromResult<IEnumerable<CurrencyInterestRate>>(
                            new CurrencyInterestRate[]
                            {
								new CurrencyInterestRate(
									countryCode: "US",
									currencyCode: "USD",
									centralBank: "Federal Reserve System",
									interestRate: 5.0m,
									latestChangeDate: DateTime.Now,
									latestChangeDiff: -1.0m
								),
                            }
                        ));
                });

			services.AddSingleton(serviceMock.Object);

			// We use AddHostedService to register workers in the app's DI container.
			// But to improve tests quality and trigger worker manually, AddSingleton is called instead.
			services.AddSingleton<CurrencyInterestRatesRefreshWorker>();

			ServiceProvider serviceProvider = services.BuildServiceProvider();
			_worker = serviceProvider.GetRequiredService<CurrencyInterestRatesRefreshWorker>();
			_service = serviceProvider.GetRequiredService<ICurrencyInterestRatesService>();
		}

		[Test]
		public async Task RefreshWorker_Should_GetCurrencyInterestRatesInBackground()
		{
			await _worker.StartAsync(CancellationToken.None);
			IEnumerable<CurrencyInterestRate> currencyInterestRates = await _service.GetCurrencyInterestRates();
			Assert.IsNotEmpty(currencyInterestRates);
		}
	}
}