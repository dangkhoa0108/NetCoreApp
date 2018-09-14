using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Data.Enums;
using CoreApp.Data.Interfaces;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Languages")]
    public class Language:DomainEntity<string>, ISwitchable
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        public bool IsDefault { get; set; }
        public string Resources { get; set; }
        public Status Status { get; set; }
    }
}
