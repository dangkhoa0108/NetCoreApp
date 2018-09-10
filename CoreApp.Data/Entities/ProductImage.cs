using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("ProductImages")]
    public class ProductImage:DomainEntity<int>
    {
        public int ProductId { get; set; }
        [StringLength(250)]
        public string Path { get; set; }
        [StringLength(250)]
        public string Caption { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
    }
}
