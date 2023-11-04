namespace ForexHelpers.Web.Services
{
	public class CurrencyInterestRatesRefreshWorker : BackgroundService
	{
		private readonly TimeSpan _refreshInterval = TimeSpan.FromHours(6);
		private readonly ICurrencyInterestRatesService _currencyInterestRatesService;

		public CurrencyInterestRatesRefreshWorker(ICurrencyInterestRatesService currencyInterestRatesService)
		{
			_currencyInterestRatesService = currencyInterestRatesService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await _currencyInterestRatesService.RefreshCurrencyInterestRates();
				await Task.Delay(_refreshInterval, stoppingToken);
			}

		}
	}
}