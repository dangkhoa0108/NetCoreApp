using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Footers")]
    public class Footer:DomainEntity<string>
    {
        [Required]
        public string Content { get; set; }
    }
}
