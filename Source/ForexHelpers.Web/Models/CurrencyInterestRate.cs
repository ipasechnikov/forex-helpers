namespace ForexHelpers.Web.Models
{
	public class CurrencyInterestRate
	{
		public CurrencyInterestRate(
			string countryCode,
			string currencyCode,
			string centeralBank,
			decimal interestRate,
			DateTime latestChangeDate,
			decimal latestChangeDiff
		)
		{
			CountryCode = countryCode;
			CurrencyCode = currencyCode;
			CenteralBank = centeralBank;
			InterestRate = interestRate;
			LatestChangeDate = latestChangeDate;
			LatestChangeDiff = latestChangeDiff;
		}

		public string CountryCode { get; }
		public string CurrencyCode { get; }
		public string CenteralBank { get; }
		public decimal InterestRate { get; }
		public DateTime LatestChangeDate { get; }
		public decimal LatestChangeDiff { get; }
	}
}