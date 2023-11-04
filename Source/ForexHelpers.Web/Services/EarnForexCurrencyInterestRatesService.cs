using System.Text.Json;
using System.Text.RegularExpressions;

using ForexHelpers.Web.Configs;
using ForexHelpers.Web.Models;

using HtmlAgilityPack;

namespace ForexHelpers.Web.Services
{
	public class EarnForexCurrencyInterestRatesService : ICurrencyInterestRatesService
	{
		public IEnumerable<CurrencyInterestRate> CurrencyInterestRates
		{
			get; private set;
		} = Array.Empty<CurrencyInterestRate>();

		public async Task GetCurrencyInterestRates()
		{
			CurrencyInterestRates = await ParseCurrencyInterestRates();
		}

		private async Task<IEnumerable<CurrencyInterestRate>> ParseCurrencyInterestRates()
		{
			HtmlWeb web = new();
			HtmlDocument doc = await web.LoadFromWebAsync("https://www.earnforex.com/interest-rates-table/");

			HtmlNodeCollection currentInterestRatesTableRows = doc.DocumentNode.SelectNodes("//div[@class='rates__table-row']");
			IEnumerable<CurrencyInterestRate> currencyInterestRates = currentInterestRatesTableRows.Select(ParseCurrencyInterestRatesTableRow);
			return currencyInterestRates;
		}

		private CurrencyInterestRate ParseCurrencyInterestRatesTableRow(HtmlNode tableRow)
		{
			HtmlNodeCollection cells = tableRow.SelectNodes("/div[@class='rates__table-col']");
			HtmlNode countryCell = cells[0];
			HtmlNode currentRateCell = cells[1];
			HtmlNode latestChangeCell = cells[2];
			HtmlNode centralBankCell = cells[3];

			throw new NotImplementedException();
		}

		private string ParseCountryCode(HtmlNode countryCell)
		{
			string countyFlagImgSrc = countryCell
				.SelectSingleNode("/img")
				.GetAttributeValue("src", string.Empty);

			Match match = Regex.Match(countyFlagImgSrc, @".*/flags/(\w{2})\.gif");
			if (!match.Success)
			{
				throw new Exception("Failed to get country code from flag image name");
			}

			string countryCode = match.Groups[1].Value.ToUpper();
			return countryCode;
		}

		private decimal ParseCurrentRateCell(HtmlNode currentRateCell)
		{
			string currentRatePattern = @"(\-?\d+\.\d+)%";
			MatchCollection matches = Regex.Matches(currentRateCell.InnerText, currentRatePattern);

			// Sometimes an interest rate is written as an interval (e.g. "5.25% — 5.50%")
			decimal averageCurrentRate = matches.Select(match =>
			{
				string currentRateStr = match.Groups[1].Value;
				decimal currentRate = decimal.Parse(currentRateStr);
				return currentRate;
			}).Average();

			return averageCurrentRate;
		}

		private void ParseLatestChangeCell(
			HtmlNode latestChangeCell,
			out DateTime latestChangeDate,
			out decimal latestChangeDiff
		)
		{
			throw new NotImplementedException();
		}
	}
}