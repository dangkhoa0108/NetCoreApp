using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Slides")]
    public class Slide:DomainEntity<int>
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        [StringLength(250)]
        public string Image { get; set; }
        public string Url { get; set; }
        public int? DisplayOrder { get; set; }
        public bool Status { get; set; }
        public string Content { get; set; }
        [Required]
        [StringLength(25)]
        public string GroupAlias { get; set; }
    }
}
