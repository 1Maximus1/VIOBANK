using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Twilio.Http;
using VIOBANK.Application.Services;

namespace VIOBANK.Infrastructure
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory) 
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null) 
            {
                return; 
            }

            if (!int.TryParse(userIdClaim.Value, out int userId))
            {
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<PermissionService>();

            var permissions = await permissionService.GetPermissionsAsync(userId);

            if (permissions == null)
            {
                return;
            }
            if (permissions.Intersect(requirement.Permissions).Any())
            {
                context.Succeed(requirement);
            }
        }
    }
}
