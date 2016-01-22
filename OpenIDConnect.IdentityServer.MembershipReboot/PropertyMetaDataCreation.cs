using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenIDConnect.Core.Models.UserManagement;
using OpenIDConnect.IdentityServer.MembershipReboot.Extensions;
using OpenIDConnect.IdentityServer.MembershipReboot.Models;

namespace OpenIDConnect.IdentityServer.MembershipReboot
{
    public static class PropertyMetaDataCreation
    {
        public static IEnumerable<PropertyMetadata> FromType<T>()
        {
            return FromType(typeof(T), new string[0]);
        }

        public static IEnumerable<PropertyMetadata> FromType(Type type, params string[] propertiesToExclude)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            List<PropertyMetadata> props = new List<PropertyMetadata>();

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly);
            foreach (PropertyInfo property in properties)
            {
                if (!propertiesToExclude.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                {
                    if (property.IsValidAsPropertyMetadata())
                    {
                        var propMeta = FromPropertyInfo(
                            property.GetPropertyDataType(),
                            property.Name,
                            property.GetName(),
                            property.IsRequired(),
                            property);
                        props.Add(propMeta);
                    }
                }
            }

            return props;
        }

        public static PropertyMetadata FromPropertyInfo(string displayFieldType, string claimType, string name, bool required, PropertyInfo property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.IsValidAsPropertyMetadata())
            {
                throw new InvalidOperationException(property.Name + " is an invalid property for use as PropertyMetadata");
            }

            return new ReflectedPropertyMetadata(
                displayFieldType,
                name,
                claimType,
                required,
                property);
        }
    }
}
