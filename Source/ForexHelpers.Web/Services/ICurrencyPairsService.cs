using ForexHelpers.Web.Models;

namespace ForexHelpers.Web.Services
{
	public interface ICurrencyPairsService
	{
		Task<IEnumerable<CurrencyPair>> GetCurrencyPairs();
	}
}