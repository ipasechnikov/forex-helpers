using ForexHelpers.Web.Models;

namespace ForexHelpers.Web.Services
{
	public interface ICurrencyInterestRatesService
	{
		IEnumerable<CurrencyInterestRate> CurrencyInterestRates { get; }
		Task GetCurrencyInterestRates();
	}
}