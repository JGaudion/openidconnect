using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenIDConnect.Core.Models.UserManagement;

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

            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.DeclaredOnly);
            foreach (var property in properties)
            {
                if (!propertiesToExclude.Contains(property.Name, StringComparer.OrdinalIgnoreCase))
                {
                    if (property.IsValidAsPropertyMetadata())
                    {
                        var propMeta = FromPropertyInfo(property);
                        props.Add(propMeta);
                    }
                }
            }

            return props;
        }

        public static PropertyMetadata FromPropertyInfo(
            PropertyInfo property,
            string type = null,
            string name = null,
            string dataType = null,
            bool? required = null)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (!property.IsValidAsPropertyMetadata())
            {
                throw new InvalidOperationException(property.Name + " is an invalid property for use as PropertyMetadata");
            }

            return new ReflectedPropertyMetadata(property)
            {
                Type = type ?? property.Name,
                Name = name ?? property.GetName(),
                DataType = dataType ?? property.GetPropertyDataType(),
                Required = required ?? property.IsRequired(),
            };
        }
    }
}
