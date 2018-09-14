using CoreApp.Data.EF.Extensions;
using CoreApp.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CoreApp.Data.EF.Config
{
    public class ContactDetailConfiguration: ModelBuilderExtensions.DbEntityConfiguration<Contact>
    {
        public override void Configure(EntityTypeBuilder<Contact> entity)
        {
            entity.Property(c => c.Id).HasMaxLength(255).IsRequired();
            entity.HasKey(c => c.Id);
        }
    }
}
