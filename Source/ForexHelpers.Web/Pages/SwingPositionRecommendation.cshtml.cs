using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ForexHelpers.Web.Pages
{
    public class SwingPositionRecommendationModel : PageModel
    {
        private readonly ICurrencyPairsService _currencyPairsService;

        public SwingPositionRecommendationModel(ICurrencyPairsService currencyPairsService)
        {
            _currencyPairsService = currencyPairsService;
        }

        public IEnumerable<CurrencyPair> CurrencyPairs
        {
            get; set;
        } = Array.Empty<CurrencyPair>();

        public async Task OnGet()
        {
            CurrencyPairs = await _currencyPairsService.GetCurrencyPairs();
        }
    }
}