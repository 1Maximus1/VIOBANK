using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VIOBANK.PostgresPersistence.AuthOptions;
using VIOBANK.PostgresPersistence.Entities;
using VIOBANK.Domain.Enums;

namespace VIOBANK.PostgresPersistence.Configurations
{
    public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermissionEnity>
    {
        private readonly AuthorizationOptions _authorization;

        public RolePermissionConfiguration(AuthorizationOptions authorization)
        {
            _authorization = authorization;
        }

        public void Configure(EntityTypeBuilder<RolePermissionEnity> builder)
        {
            builder.HasKey(r => new { r.RoleId, r.PermissionId });

            builder.HasData(ParseRolePermission());
        }
        
        private RolePermissionEnity[] ParseRolePermission()
        {
            if (_authorization.RolePermissions == null || _authorization.RolePermissions.Length == 0)
            {
                throw new Exception("AuthorizationOptions.RolePermissions is empty. Check appsettings.json.");
            }

            var array = _authorization.RolePermissions
             .SelectMany(rp => rp.Permissions
                .Select(p => new RolePermissionEnity
                {
                    RoleId = (int)Enum.Parse<RoleEnum>(rp.Role),
                    PermissionId = (int)Enum.Parse<PermissionEnum>(p)
                })).ToArray();

            if (array.Length == 0)
            {
                throw new Exception("Parsed RolePermissions is empty.");
            }

            return array;
        }

    }
}
