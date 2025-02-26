using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VIOBANK.Domain.Models;

namespace VIOBANK.PostgresPersistence.Configurations
{
    public class MobileTopupConfiguration : IEntityTypeConfiguration<MobileTopup>
    {
        public void Configure(EntityTypeBuilder<MobileTopup> builder)
        {
            builder.HasKey(m => m.TopupId);

            builder.Property(m => m.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(m => m.Amount)
                .IsRequired();

            builder.HasOne(m => m.User)
                .WithMany(u => u.MobileTopups)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
