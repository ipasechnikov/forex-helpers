namespace ForexHelpers.Web.Models
{
	public class CurrencyInterestRate
	{
		public CurrencyInterestRate(
			string countryCode,
			string currencyCode,
			string centralBank,
			decimal interestRate,
			DateTime latestChangeDate,
			decimal latestChangeDiff
		)
		{
			CountryCode = countryCode;
			CurrencyCode = currencyCode;
			CentralBank = centralBank;
			InterestRate = interestRate;
			LatestChangeDate = latestChangeDate;
			LatestChangeDiff = latestChangeDiff;
		}

		public string CountryCode { get; }
		public string CurrencyCode { get; }
		public string CentralBank { get; }
		public decimal InterestRate { get; }
		public DateTime LatestChangeDate { get; }
		public decimal LatestChangeDiff { get; }
	}
}