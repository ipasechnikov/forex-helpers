namespace ForexHelpers.Web.Services
{
	public class GetLatestCurrencyInterestRatesWorker : BackgroundService
	{
		private const int REFRESH_FREQUENCY_HOURS = 6;
		private readonly ICurrencyInterestRatesService _currencyInterestRatesService;

		public GetLatestCurrencyInterestRatesWorker(ICurrencyInterestRatesService currencyInterestRatesService)
		{
			_currencyInterestRatesService = currencyInterestRatesService;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			await _currencyInterestRatesService.GetCurrencyInterestRates();
			await Task.Delay(TimeSpan.FromHours(REFRESH_FREQUENCY_HOURS), stoppingToken);
		}
	}
}