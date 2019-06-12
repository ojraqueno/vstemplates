using Microsoft.AspNetCore.Identity;
using System;

namespace Core1.Model
{
    public class AppIdentityUser : IdentityUser
    {
        public DateTime AddedOn { get; set; }
        public DateTime? DeletedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? TimezoneOffsetMinutes { get; set; }
    }
}