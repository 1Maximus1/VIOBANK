using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Enums;
using VIOBANK.Domain.Models;

namespace VIOBANK.Infrastructure
{
    public class PermissionRequirement(PermissionEnum[] permissions) : IAuthorizationRequirement
    {
        public PermissionEnum[] Permissions { get; set; } = permissions;
    }
}
