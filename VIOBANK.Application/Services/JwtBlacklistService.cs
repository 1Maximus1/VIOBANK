using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VIOBANK.Domain.Stores;

namespace VIOBANK.Application.Services
{
    public class JwtBlacklistService
    {
        private readonly IBlacklistedTokenRepository _dbContext;
        private static readonly int EXPIRY_HOURS = 12;

        public JwtBlacklistService(IBlacklistedTokenRepository dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToBlacklistAsync(string token)
        {
            await _dbContext.AddToBlacklistAsync(token, EXPIRY_HOURS);
        }

        public async Task<bool> IsBlacklistedAsync(string token)
        {
            return await _dbContext
                .IsBlacklistedAsync(token);
        }
    }
}
