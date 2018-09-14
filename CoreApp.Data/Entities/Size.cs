using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Sizes")]
    public class Size:DomainEntity<int>
    {
        [StringLength(250)]
        public string Name { get; set; }
    }
}
