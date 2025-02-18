using Microsoft.EntityFrameworkCore;
using VIOBANK.Domain.Stores;
using VIOBANK.PostgresPersistence.Entities;

namespace VIOBANK.PostgresPersistence.Repositories
{
    public class BlacklistedTokenRepository : IBlacklistedTokenRepository
    {
        private readonly VIOBANKDbContext _dbContext;

        public BlacklistedTokenRepository(VIOBANKDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddToBlacklistAsync(string token, int expiryHours = 12)
        {
            var blacklistedToken = new BlacklistedToken
            {
                Token = token,
                ExpiryDate = DateTime.UtcNow.AddHours(expiryHours)
            };

            _dbContext.BlacklistedTokens.Add(blacklistedToken);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> IsBlacklistedAsync(string token)
        {
            return await _dbContext.BlacklistedTokens
                .AnyAsync(t => t.Token == token && t.ExpiryDate > DateTime.UtcNow);
        }

        public async Task RemoveExpiredTokensAsync()
        {
            var expiredTokens = await _dbContext.BlacklistedTokens
                .Where(t => t.ExpiryDate <= DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _dbContext.BlacklistedTokens.RemoveRange(expiredTokens);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
