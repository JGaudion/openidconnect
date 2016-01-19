using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenIDConnect.Core.Models.UserManagement
{
    public class UserManagementResult
    {
        public static UserManagementResult Success
        {
            get
            {
                return new UserManagementResult();
            }
        }

        public UserManagementResult()
        {
        }

        public UserManagementResult(IEnumerable<string> errors)
        {
            if (errors == null)
            {
                throw new ArgumentNullException(nameof(errors));
            }
        }

        public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();

        public bool IsError
        {
            get
            {
                return this.Errors.Any();
            }
        }
    }

    public class UserManagementResult<TResult> : UserManagementResult
    {
        public UserManagementResult(TResult result)
        {
            this.Result = result;
        }

        public UserManagementResult(IEnumerable<string> errors)
            : base(errors)
        {

        }

        public TResult Result { get; }
    }
}
