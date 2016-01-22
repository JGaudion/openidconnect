using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenIDConnect.Core.Models.UserManagement;

namespace OpenIDConnect.IdentityServer.MembershipReboot.Models
{
    public abstract class ExecutablePropertyMetadata : PropertyMetadata
    {
        protected ExecutablePropertyMetadata(string displayFieldType, string claimType, string name, bool required) 
            : base(displayFieldType, claimType, name, required)
        {
        }

        public abstract string Get(object instance);
        public abstract UserManagementResult Set(object instance, string value);
    }
}
