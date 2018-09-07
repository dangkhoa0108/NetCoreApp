using CoreApp.Data.EF.Extensions;
using CoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApp.Data.EF.Config
{
    public class TagConfig: ModelBuilderExtensions.DbEntityConfiguration<Tag>
    {
        /// <summary>
        /// Vì ID của Tag là kiểu string kế thừa abstract class nên không thể cấu hình độ dài và kiểu dữ liệu
        /// Nên đây là cách cấu hình.
        /// </summary>
        /// <param name="entity"></param>
        public override void Configure(EntityTypeBuilder<Tag> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(50).IsRequired().HasColumnType("varchar(50)");
        }
    }
}
