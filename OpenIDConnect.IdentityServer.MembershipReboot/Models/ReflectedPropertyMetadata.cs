using System;
using System.Reflection;
using OpenIDConnect.Core.Constants;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Models
{
    public class ReflectedPropertyMetadata : ExecutablePropertyMetadata
    {
        private readonly PropertyInfo _property;

        public ReflectedPropertyMetadata(
            string displayFieldName,
            string claimType,
            string name,
            bool required,
            PropertyInfo property
            ) 
            : base(displayFieldName, claimType, name, required)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            _property = property;
        }

        public override string Get(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (DisplayFieldType == PropertyTypes.Password)
            {
                return null;
            }

            var value = _property.GetValue(instance);
            return value?.ToString();
        }

        public override UserManagementResult Set(object instance, string value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                _property.SetValue(instance, null);
            }
            else
            {
                var type = Nullable.GetUnderlyingType(_property.PropertyType) ?? _property.PropertyType;
                object convertedValue;
                try
                {
                    convertedValue = Convert.ChangeType(value, type);
                }
                catch
                {
                    return new UserManagementResult(new[] { "ConversionFailed" });
                }

                _property.SetValue(instance, convertedValue);
            }

            return UserManagementResult.Success;
        }
    }
}
