using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace ForexHelpers.Tests
{
	internal class Configuration : IConfiguration
	{
		private readonly IConfiguration _config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.test.json")
			.Build();

		public string? this[string key]
		{
			get => _config[key];
			set => _config[key] = value;
		}

		public IEnumerable<IConfigurationSection> GetChildren()
		{
			return _config.GetChildren();
		}

		public IChangeToken GetReloadToken()
		{
			return _config.GetReloadToken();
		}

		public IConfigurationSection GetSection(string key)
		{
			return _config.GetSection(key);
		}
	}
}