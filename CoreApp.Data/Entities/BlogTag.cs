using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("BlogTags")]
    public class BlogTag:DomainEntity<int>
    {
        public int BlogId { get; set; }
        public string TagId { get; set; }
        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }
        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }  
    }
}
