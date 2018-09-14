using System;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Data.Enums;
using CoreApp.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoreApp.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser:IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public string FullName { get; set; }
        public DateTime?  Birthday { get; set; }
        public Decimal Balance { get; set; }
        public string Avatar { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Status Status { get; set; }
    }
}
