namespace ForexHelpers.Web.Configs
{
	public class AppSettingMissingException : Exception
	{
		public AppSettingMissingException(string? missingSetting)
			: base($"'{missingSetting}' setting is missing in 'appsettings.json'")
		{
		}
	}
}
