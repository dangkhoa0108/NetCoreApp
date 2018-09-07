using System.ComponentModel.DataAnnotations;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    public class Tag: DomainEntity<string>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
