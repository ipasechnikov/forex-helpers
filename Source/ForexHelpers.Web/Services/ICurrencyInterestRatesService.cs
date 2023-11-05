using ForexHelpers.Web.Models;

namespace ForexHelpers.Web.Services
{
	public interface ICurrencyInterestRatesService
	{
		Task<IEnumerable<CurrencyInterestRate>> GetCurrencyInterestRates();
		Task<CurrencyInterestRate> GetCurrencyInterestRate(string currencyCode);
		Task RefreshCurrencyInterestRates();
	}
}