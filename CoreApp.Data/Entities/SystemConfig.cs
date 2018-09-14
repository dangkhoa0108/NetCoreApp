using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Data.Enums;
using CoreApp.Data.Interfaces;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("SystemConfigs")]
    public class SystemConfig: DomainEntity<string>, ISwitchable
    {
        [Required]
        [StringLength(128)]
        public string Name { get; set; }
        public string Value1 { get; set; }
        public int? Value2 { get; set; }
        public bool? Value3 { get; set; }
        public decimal? Value4 { get; set; }
        public DateTime? Value5 { get; set; }
        public Status Status { get; set; }
    }
}
