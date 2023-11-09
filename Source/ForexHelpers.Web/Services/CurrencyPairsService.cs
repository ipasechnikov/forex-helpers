using ForexHelpers.Web.Models;

namespace ForexHelpers.Web.Services
{
	public class CurrencyPairsService : ICurrencyPairsService
	{
		// List of widely used Forex currency pairs
		private readonly (string, string)[] _currencyPairs = new (string, string)[]
		{
			// Majors
			("EUR", "USD"),
			("USD", "JPY"),
			("GBP", "USD"),
			("USD", "CHF"),
			("AUD", "USD"),
			("USD", "CAD"),
			("NZD", "USD"),

			// Minors
			("EUR", "GBP"),
			("EUR", "CHF"),
			("EUR", "CAD"),
			("EUR", "AUD"),
			("EUR", "NZD"),
			("EUR", "JPY"),
			("CHF", "JPY"),
			("AUD", "JPY"),
			("GBP", "JPY"),
			("CAD", "JPY"),
			("NZD", "JPY"),
			("GBP", "AUD"),
			("GBP", "CHF"),
			("GBP", "CAD"),
			("GBP", "NZD"),
			("CAD", "CHF"),
			("AUD", "CAD"),
			("NZD", "CAD"),
			("AUD", "CHF"),
			("AUD", "NZD"),
			("NZD", "CHF"),

			// Exotics
			("USD", "HKD"),
			("USD", "SGD"),
			("USD", "ZAR"),
			("USD", "YHB"),
			("USD", "MXN"),
			("USD", "DKK"),
			("USD", "SEK"),
			("USD", "NOK"),
			("USD", "INR"),
		};

		private readonly ICurrencyInterestRatesService _currencyInterestRatesService;

		public CurrencyPairsService(ICurrencyInterestRatesService currencyInterestRatesService)
		{
			_currencyInterestRatesService = currencyInterestRatesService;
		}

		public async Task<IEnumerable<CurrencyPair>> GetCurrencyPairs()
		{
			List<CurrencyPair> currencyPairs = new();
			foreach ((string baseCurrency, string quoteCurrency) in _currencyPairs)
			{
                CurrencyInterestRate? baseCurrencyInterestRate = await _currencyInterestRatesService.GetCurrencyInterestRate(baseCurrency);
                CurrencyInterestRate? quoteCurrencyInterestRate = await _currencyInterestRatesService.GetCurrencyInterestRate(quoteCurrency);
				if (baseCurrencyInterestRate is null || quoteCurrencyInterestRate is null)
				{
					continue;
				}

				currencyPairs.Add(new CurrencyPair(
                    baseCurrency,
                    baseCurrencyInterestRate.InterestRate,
                    quoteCurrency,
                    quoteCurrencyInterestRate.InterestRate
                ));
            }

			return currencyPairs;
		}
	}
}