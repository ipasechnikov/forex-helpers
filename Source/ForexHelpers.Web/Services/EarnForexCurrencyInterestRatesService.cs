using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using ForexHelpers.Web.Models;
using HtmlAgilityPack;
using RESTCountries.NET.Models;
using RESTCountries.NET.Services;

namespace ForexHelpers.Web.Services
{
	public class EarnForexCurrencyInterestRatesService : ICurrencyInterestRatesService
	{
		private IDictionary<string, CurrencyInterestRate> _currencyInterestRates = new Dictionary<string, CurrencyInterestRate>();

		public async Task<CurrencyInterestRate?> GetCurrencyInterestRate(string currencyCode)
		{
			if (_currencyInterestRates.Count == 0)
			{
				await RefreshCurrencyInterestRates();
			}

			Trace.Assert(_currencyInterestRates.Count != 0);
			_currencyInterestRates.TryGetValue(currencyCode, out CurrencyInterestRate? currencyInterestRate);
			return currencyInterestRate;
		}

		public async Task<IEnumerable<CurrencyInterestRate>> GetCurrencyInterestRates()
		{
			if (_currencyInterestRates.Count == 0)
			{
				await RefreshCurrencyInterestRates();
			}

			Trace.Assert(_currencyInterestRates.Count != 0);
			return _currencyInterestRates.Values;
		}

		public async Task RefreshCurrencyInterestRates()
		{
			CurrencyInterestRate[] currencyInterestRates = await ParseCurrencyInterestRates();
			_currencyInterestRates = currencyInterestRates
				.ToDictionary(
					currencyInterestRate => currencyInterestRate.CurrencyCode,
					currencyInterestRate => currencyInterestRate
				);
		}

		private string GetCurrencyCodeByCountryCode(string countryCode)
		{
			Country? country = RestCountriesService.GetCountryByCode(countryCode);
			if (country is null)
			{
				throw new Exception($"No country with code '{countryCode}' was found");
			}

			// If a country have multiple currencies, remove Euro if present
			string? currencyCode = country.Currencies?.Keys
				.Where(code => code != "EUR")
				.First();
			if (currencyCode is null)
			{
				throw new Exception($"No currency for country code '{countryCode} was found'");
			}

			return currencyCode;
		}

		private void ParseCentralBankCell(HtmlNode centralBankCell, out string centralBank)
		{
			centralBank = centralBankCell.SelectSingleNode("./a").InnerText;
		}

		private void ParseCountryCell(HtmlNode countryCell, out string countryCode, out string currencyCode)
		{
			string countyFlagImgSrc = countryCell
				.SelectSingleNode("./img")
				.GetAttributeValue("src", string.Empty);

			Match match = Regex.Match(countyFlagImgSrc, @".*/flags/(\w{2})\.gif");
			if (!match.Success)
			{
				throw new Exception("Failed to get country code from flag image name");
			}

			countryCode = match.Groups[1].Value.ToUpper();

			// Special case for European Union because it's not a country but an union
			currencyCode = countryCode == "EU" ? "EUR" : GetCurrencyCodeByCountryCode(countryCode);
		}

		private async Task<CurrencyInterestRate[]> ParseCurrencyInterestRates()
		{
			HtmlWeb web = new();
			HtmlDocument doc = await web.LoadFromWebAsync("https://www.earnforex.com/interest-rates-table/");

			HtmlNodeCollection currentInterestRatesTableRows = doc.DocumentNode.SelectNodes("//div[@class='rates__table-row']");
			CurrencyInterestRate[] currencyInterestRates = currentInterestRatesTableRows.Select(ParseCurrencyInterestRatesTableRow).ToArray();
			return currencyInterestRates;
		}

		private CurrencyInterestRate ParseCurrencyInterestRatesTableRow(HtmlNode tableRow)
		{
			HtmlNodeCollection cells = tableRow.SelectNodes("./div[@class='rates__table-col']");
			HtmlNode countryCell = cells[0];
			HtmlNode currentRateCell = cells[1];
			HtmlNode latestChangeCell = cells[2];
			HtmlNode centralBankCell = cells[3];

			ParseCountryCell(countryCell, out string countryCode, out string currencyCode);
			ParseCurrentRateCell(currentRateCell, out decimal interestRate);
			ParseLatestChangeCell(latestChangeCell, out DateTime latestChangeDate, out decimal latestChangeDiff);
			ParseCentralBankCell(centralBankCell, out string centralBank);

			return new CurrencyInterestRate(
				countryCode,
				currencyCode,
				centralBank,
				interestRate,
				latestChangeDate,
				latestChangeDiff
			);
		}

		private void ParseCurrentRateCell(HtmlNode currentRateCell, out decimal interestRate)
		{
			MatchCollection matches = Regex.Matches(
				currentRateCell.GetDirectInnerText().Trim(), @"(\-?\d+\.\d+)%"
			);

			// Sometimes an interest rate is stored as a range (e.g. "5.25% — 5.50%")
			interestRate = matches.Select(match =>
			{
				string currentRateStr = match.Groups[1].Value;
				decimal currentRate = decimal.Parse(currentRateStr);
				return currentRate;
			}).Average();
		}

		private void ParseLatestChangeCell(
			HtmlNode latestChangeCell,
			out DateTime latestChangeDate,
			out decimal latestChangeDiff
		)
		{
			string changeDiffClass = latestChangeCell
				.SelectSingleNode("./span[contains(@class, 'rates__moving')]")
				.GetAttributeValue("class", string.Empty);

			if (string.IsNullOrEmpty(changeDiffClass))
			{
				throw new Exception($"Failed to parse {nameof(changeDiffClass)}");
			}

			int changeDiffSign;
			if (changeDiffClass.Contains("increased"))
			{
				changeDiffSign = 1;
			}
			else if (changeDiffClass.Contains("decreased"))
			{
				changeDiffSign = -1;
			}
			else
			{
				throw new Exception($"Failed to parse {nameof(changeDiffSign)}");
			}

			Match match = Regex.Match(
				latestChangeCell.GetDirectInnerText().Trim(), @"(\d{4}-\d{2}-\d{2})\s+by\s+(\d+.\d+)%"
			);

			if (!match.Success)
			{
				throw new Exception($"Failed to parse {nameof(latestChangeDate)} and {nameof(latestChangeDiff)}");
			}

			string latestChangeDateStr = match.Groups[1].Value;
			latestChangeDate = DateTime.ParseExact(latestChangeDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture);

			string latestChangeDiffStr = match.Groups[2].Value;
			latestChangeDiff = changeDiffSign * decimal.Parse(latestChangeDiffStr);
		}
	}
}