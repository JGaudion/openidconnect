namespace OpenIDConnect.Core
{
    public interface IConfigurationService
    {
        TValue GetSetting<TValue>(string settingName, TValue defaultValue);
    }
}