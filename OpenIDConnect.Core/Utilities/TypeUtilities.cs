using System;
using System.Reflection;
using OpenIDConnect.Core.Constants;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.Core.Utilities
{
    public static class TypeUtilities
    {
        public static UserManagementResult SetProperty(object instance, string propertyName, string value, bool ignorePropertiesWhichDoNotExist = false)
        {
            try
            {
                Type type = instance.GetType();
                PropertyInfo prop = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

                //Option to let us completely ignore properties which don't exist, rather than raising an error
                if(ignorePropertiesWhichDoNotExist && prop == null)
                {
                    return UserManagementResult.Success;
                }

                Type conversionType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                object obj;
                try
                {
                    obj = Convert.ChangeType(value, conversionType);
                }
                catch
                {
                    return new UserManagementResult(new[] { $"Conversion of {value} to {conversionType.FullName} failed." });
                }

                if (string.IsNullOrWhiteSpace(value))
                {
                    prop.SetValue(instance, null);
                }
                else
                {
                    prop.SetValue(instance, obj);
                }

                return UserManagementResult.Success;
            }
            catch (Exception e)
            {
                throw new Exception("Error setting property type " + propertyName, e);
            }
        }

        public static string GetProperty(object instance, PropertyMetadata propertyMetadata, bool ignorePropertiesWhichDoNotExist = false)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            if (propertyMetadata.DisplayFieldType == PropertyTypes.Password)
            {
                return null;
            }

            Type type = instance.GetType();
            PropertyInfo prop = type.GetProperty(propertyMetadata.ClaimType, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            //Option to let us completely ignore properties which don't exist, rather than raising an error
            if (ignorePropertiesWhichDoNotExist && prop == null)
            {
                return string.Empty;
            }

            var value = prop.GetValue(instance);

            return value?.ToString();
        }
    }
}
