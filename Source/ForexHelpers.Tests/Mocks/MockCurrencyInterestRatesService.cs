using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ForexHelpers.Web.Models;
using ForexHelpers.Web.Services;

namespace ForexHelpers.Tests.Mocks
{
    internal class MockCurrencyInterestRatesService : ICurrencyInterestRatesService
    {
        private IDictionary<string, CurrencyInterestRate> _currencyInterestRates = new Dictionary<string, CurrencyInterestRate>();

        public async Task<CurrencyInterestRate?> GetCurrencyInterestRate(string currencyCode)
        {
            if (_currencyInterestRates.Count == 0)
            {
                await RefreshCurrencyInterestRates();
            }

            _currencyInterestRates.TryGetValue(currencyCode, out CurrencyInterestRate? currencyInterestRate);
            return currencyInterestRate;
        }

        public async Task<IEnumerable<CurrencyInterestRate>> GetCurrencyInterestRates()
        {
            if (_currencyInterestRates.Count == 0)
            {
                await RefreshCurrencyInterestRates();
            }

            return _currencyInterestRates.Values;
        }

        public Task RefreshCurrencyInterestRates()
        {
            CurrencyInterestRate[] currencyInterestRates = new CurrencyInterestRate[]
            {
                new CurrencyInterestRate(
                    countryCode: "US",
                    currencyCode: "USD",
                    centralBank: "Central Bank 1",
                    interestRate: 2.0m,
                    latestChangeDate: DateTime.Now,
                    latestChangeDiff: 3.0m
                ),
                new CurrencyInterestRate(
                    countryCode: "EU",
                    currencyCode: "EUR",
                    centralBank: "Central Bank 2",
                    interestRate: 3.0m,
                    latestChangeDate: DateTime.Now,
                    latestChangeDiff: 4.0m
                ),
            };

            _currencyInterestRates = currencyInterestRates.ToDictionary(
                x => x.CurrencyCode,
                x => x
            );

            return Task.CompletedTask;
        }
    }
}
