using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VIOBANK.PostgresPersistence.Entities;

namespace VIOBANK.PostgresPersistence.Configurations
{
    public class BlacklistedTokenConfiguration : IEntityTypeConfiguration<BlacklistedToken>
    {
        public void Configure(EntityTypeBuilder<BlacklistedToken> builder)
        {
            builder.ToTable("BlacklistedTokens"); 

            builder.HasKey(b => b.Token); 

            builder.Property(b => b.Token)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.ExpiryDate)
                .IsRequired(); 

            builder.HasIndex(b => b.ExpiryDate); 
        }
    }
}
