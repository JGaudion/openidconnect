using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.IdentityServer.MembershipReboot
{
    public static class PropertyMetaDataCreation
    {
        public static PropertyMetadata FromFunctions<TContainer, TProperty>(
            string type,
            Func<TContainer, TProperty> get,
            Func<TContainer, TProperty, UserManagementResult> set,
            string name = null,
            string dataType = null,
            bool? required = null)
        {
            if (String.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (get == null)
            {
                throw new ArgumentNullException(nameof(get));
            }

            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            var meta = new ExpressionPropertyMetadata<TContainer, TProperty>(type, get, set);
            if (name != null) meta.Name = name;
            if (dataType != null) meta.DataType = dataType.Value;
            if (required != null) meta.Required = required.Value;

            return meta;
        }

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
    }
}
