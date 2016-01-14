using System;
using System.Configuration;

namespace OpenIDConnect.Core
{
    public class ApplicationSettingsConfigurationService : IConfigurationService
    {
        public TValue GetSetting<TValue>(string settingName, TValue defaultValue)
        {
            if (string.IsNullOrWhiteSpace(settingName))
            {
                return defaultValue;
            }

            var value = ConfigurationManager.AppSettings[settingName];
            if (value == null)
            {
                return defaultValue;
            }

            try
            {
                return (TValue)Convert.ChangeType(value, typeof(TValue));
            }
            catch
            {
                return defaultValue;
            }
        }
    }
}