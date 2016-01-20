using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.Core.Utilities
{
    public static class TypeUtilities
    {
        public static UserManagementResult SetProperty(object instance, string propertyName, string value)
        {
            try
            {
                Type type = instance.GetType();
                PropertyInfo prop = type.GetProperty(propertyName, BindingFlags.Public);
                
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

    }
}
