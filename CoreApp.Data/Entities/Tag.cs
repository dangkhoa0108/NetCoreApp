using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Tags")]
    public class Tag: DomainEntity<string>
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
