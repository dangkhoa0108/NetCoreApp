using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CoreApp.Infrastructure.ShareKernel;

namespace CoreApp.Data.Entities
{
    [Table("AdvertisementPages")]
    public class AdvertisementPage:DomainEntity<string>
    {
        public string Name { get; set; }    
        public virtual ICollection<AdvertisementPosition> AdvertisementPositions { get; set; }
    }
}
