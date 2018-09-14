using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Data.Enums;
using CoreApp.Data.Interfaces;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    /// <summary>
    /// Announcements table
    /// </summary>
    [Table("Announcements")]
    public class Announcement:DomainEntity<string>, ISwitchable, IDateTracking
    {
        public Announcement()
        {
            AnnouncementUsers =new List<AnnouncementUser>();
        }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        [StringLength(250)]
        public string Content { get; set; }
        [StringLength(450)]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }

        public ICollection<AnnouncementUser> AnnouncementUsers { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
