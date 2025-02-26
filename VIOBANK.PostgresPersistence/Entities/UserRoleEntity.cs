using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Models;

namespace VIOBANK.PostgresPersistence.Entities
{
    public class UserRoleEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }
    }
}
