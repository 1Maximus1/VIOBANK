using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VIOBANK.Domain.Stores
{
    public interface IBlacklistedTokenRepository
    {
        Task AddToBlacklistAsync(string token, int expiryHours = 12);
        Task<bool> IsBlacklistedAsync(string token);
        Task RemoveExpiredTokensAsync();
    }
}
