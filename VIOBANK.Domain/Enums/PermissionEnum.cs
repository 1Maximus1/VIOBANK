using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIOBANK.Domain.Enums
{
    public enum PermissionEnum
    {
        Read = 1,
        Create = 2,
        Update = 3,
        Delete = 4,
        ManageUsers = 5, 
        ManageCards = 6, 
        ManageDeposits = 7, 
        ManageTransactions = 8 
    }
}
