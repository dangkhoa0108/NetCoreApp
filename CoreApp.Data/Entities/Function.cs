using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Data.Enums;
using CoreApp.Data.Interfaces;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("Functions")]
    public class Function:DomainEntity<string>, ISwitchable, ISortable
    {
        public Function()
        {

        }

        public Function(string name,string url,string parentId,string iconCss,int sortOrder)
        {
            Name = name;
            Url = url;
            ParentId = parentId;
            IconCss = iconCss;
            SortOrder = sortOrder;
            Status = Status.Active;
        }
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Url { get; set; }
        [StringLength(128)]
        public string ParentId { get; set; }
        public string IconCss { get; set; }
        public Status Status { get; set; }
        public int SortOrder { get; set; }
    }
}
