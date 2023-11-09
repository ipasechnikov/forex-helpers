namespace ForexHelpers.Web.Models
{
	public class CurrencyPair
	{
		public CurrencyPair(
			string baseCurrency,
			decimal baseInterestRate,
			string quoteCurrency,
			decimal quoteInterestRate
		)
		{
			Base = baseCurrency;
			BaseInterestRate = baseInterestRate;
			Quote = quoteCurrency;
			QuoteInterestRate = quoteInterestRate;
		}

		public string Base { get; }
		public decimal BaseInterestRate { get; }
		public string Quote { get; }
		public decimal QuoteInterestRate { get; }
		public decimal InterestRateDiff => BaseInterestRate - QuoteInterestRate;
	}
}