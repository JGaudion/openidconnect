using System;
using System.Collections.Generic;
using System.Linq;
using OpenIDConnect.Core.Models.UserManagement;
using OpenIDConnect.IdentityServer.MembershipReboot.Models;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Extensions
{
    public static class PropertyMetadataExtensions
    {
        public static bool TrySet(this IEnumerable<PropertyMetadata> properties, object instance, string type, string value, out UserManagementResult result)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            result = null;

            var executableProperty = properties.SingleOrDefault(x => x.ClaimType == type) as ExecutablePropertyMetadata;
            if (executableProperty != null)
            {
                return executableProperty.TrySet(instance, value, out result);
            }

            return false;
        }

        public static bool TrySet(this PropertyMetadata property, object instance, string value, out UserManagementResult result)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            result = null;

            var executableProperty = property as ExecutablePropertyMetadata;
            if (executableProperty != null)
            {
                result = executableProperty.Set(instance, value);
                return true;
            }

            return false;
        }

        public static bool TryGet(this PropertyMetadata property, object instance, out string value)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            var executableProperty = property as ExecutablePropertyMetadata;
            if (executableProperty != null)
            {
                value = executableProperty.Get(instance);
                return true;
            }

            value = null;
            return false;
        }
    }
}
