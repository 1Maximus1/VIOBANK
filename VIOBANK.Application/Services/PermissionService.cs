using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Enums;
using VIOBANK.Domain.Stores;

namespace VIOBANK.Application.Services
{
    public class PermissionService
    {
        private readonly IUserStore _userStore;

        public PermissionService(IUserStore userStore) 
        {
            _userStore = userStore;
        }

        public async Task<HashSet<PermissionEnum>> GetPermissionsAsync(int userId) 
        {
            return await _userStore.GetUserPermissions(userId);
        }
    }
}
