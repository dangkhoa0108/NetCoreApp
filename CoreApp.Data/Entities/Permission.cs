using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Permissions")]
    public class Permission:DomainEntity<int>
    {
        [StringLength(450)]
        [Required]
        public string RoleId { get; set; }
        [StringLength(128)]
        [Required]
        public string FunctionId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public virtual AppRole AppRole { get; set; }
        public virtual Function Function { get; set; }
    }
}
