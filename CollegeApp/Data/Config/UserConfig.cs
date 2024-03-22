using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CollegeApp.Data.Config
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(n => n.Username).IsRequired();
            builder.Property(n => n.Password).IsRequired();
            builder.Property(n => n.PasswordSalt).IsRequired();
            builder.Property(n => n.IsActive).IsRequired();
            builder.Property(n => n.IsDeleted).IsRequired();
            builder.Property(n => n.CreatedDate).IsRequired();
            builder.Property(n => n.UserTypeId).IsRequired();


            builder.HasOne(n => n.UserType)
               .WithMany(n => n.Users)
               .HasForeignKey(n => n.UserTypeId)
               .HasConstraintName("FK_Users_UserTypes");
        }
    }
}