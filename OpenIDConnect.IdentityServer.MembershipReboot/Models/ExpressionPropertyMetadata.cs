using System;
using OpenIDConnect.Core.Constants;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Models
{
    public class ExpressionPropertyMetadata<TContainer, TProperty> : ExecutablePropertyMetadata
    {
        private readonly Func<TContainer, TProperty> _get;
        private readonly Func<TContainer, TProperty, UserManagementResult> _set;

        public ExpressionPropertyMetadata(
            string dataType,
            string claimType,
            string name,
            bool required,
            Func<TContainer, TProperty> get,
            Func<TContainer, TProperty, UserManagementResult> set)
            : base(dataType, claimType, name, required)
        {
            if (String.IsNullOrWhiteSpace(claimType))
            {
                throw new ArgumentNullException(nameof(claimType));
            }

            if (String.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (get == null)
            {
                throw new ArgumentNullException(nameof(get));
            }

            if (set == null)
            {
                throw new ArgumentNullException(nameof(set));
            }

            _get = get;
            _set = set;
        }

        public override string Get(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (ClaimType == PropertyTypes.Password)
            {
                return null;
            }

            var value = _get((TContainer)instance);
            if (value != null)
            {
                return value.ToString();
            }

            return null;
        }

        public override UserManagementResult Set(object instance, string value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                return _set((TContainer)instance, default(TProperty));
            }

            var type = typeof(TProperty);
            type = Nullable.GetUnderlyingType(type) ?? type;
            TProperty convertedValue = default(TProperty);

            try
            {
                convertedValue = (TProperty)Convert.ChangeType(value, type);
            }
            catch
            {
                return new UserManagementResult(new[] { "ConversionFailed" });
            }

            return _set((TContainer)instance, convertedValue);
        }
    }

}
