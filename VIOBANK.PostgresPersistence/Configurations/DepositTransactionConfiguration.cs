using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Models;

namespace VIOBANK.PostgresPersistence.Configurations
{
    public class DepositTransactionConfiguration : IEntityTypeConfiguration<DepositTransaction>
    {
        public void Configure(EntityTypeBuilder<DepositTransaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(t => t.Deposit)
                .WithMany(d => d.DepositTransactions)
                .HasForeignKey(t => t.DepositId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
