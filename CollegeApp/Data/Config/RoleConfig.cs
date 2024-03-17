using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.RoleName).HasMaxLength(250).IsRequired();
            builder.Property(n => n.Description);
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreatedDate).IsRequired();
        }
    }
}