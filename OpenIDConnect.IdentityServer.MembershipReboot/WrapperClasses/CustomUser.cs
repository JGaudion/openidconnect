﻿using System.ComponentModel.DataAnnotations;
using BrockAllen.MembershipReboot.Relational;

namespace OpenIDConnect.IdentityServer.MembershipReboot.WrapperClasses
{
    public class CustomUser : RelationalUserAccount
    {
        [Display(Name="First Name")]
        public virtual string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public virtual string LastName { get; set; }
        public virtual int? Age { get; set; }
    }
}