using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Jwt.Taskrouter;
using VIOBANK.Domain.Enums;

namespace VIOBANK.Infrastructure
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermissionsAttribute : AuthorizeAttribute
    {
        public RequirePermissionsAttribute(params PermissionEnum[] permissions)
        {
            Permissions = permissions;
            Policy = $"Permissions:{string.Join(",", permissions)}";
        }

        public PermissionEnum[] Permissions { get; }
    }
}
