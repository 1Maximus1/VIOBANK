using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Models;
using VIOBANK.Domain.Enums;

namespace VIOBANK.PostgresPersistence.Configurations
{
    public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.HasKey(x => x.Id);

            var permissions = Enum.GetValues<PermissionEnum>().Select(p => new Permission
            {
                Id = (int)p,
                Name = p.ToString()
            });

            builder.HasData(permissions);
        }
    }
}
