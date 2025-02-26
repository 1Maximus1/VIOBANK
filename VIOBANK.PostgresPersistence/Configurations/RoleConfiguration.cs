using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Models;
using VIOBANK.PostgresPersistence.Entities;
using VIOBANK.Domain.Enums;

namespace VIOBANK.PostgresPersistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity<RolePermissionEnity>(
                    l => l.HasOne<Permission>().WithMany().HasForeignKey(e => e.PermissionId),
                    r => r.HasOne<Role>().WithMany().HasForeignKey(e => e.RoleId)
                );

            var roles = Enum.GetValues<RoleEnum>().Select(r => new Role
            {
                Id = (int) r,
                Name = r.ToString()
            });

            builder.HasData(roles);
        }
    }
}
