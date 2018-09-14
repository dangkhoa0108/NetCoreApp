using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Data.Enums;
using CoreApp.Data.Interfaces;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    /// <summary>
    /// Advertisements table
    /// </summary>
    [Table("Advertisements")]
    public class Advertisement:DomainEntity<int>, ISwitchable, ISortable
    {
        [StringLength(250)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        [StringLength(250)]
        public string Image { get; set; }
        [StringLength(50)]
        public string Url { get; set; }
        public string PositionId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified  { get; set; }
        public Status Status { get; set; }
        public int SortOrder { get; set; }
    }
}
