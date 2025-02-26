using VIOBANK.Domain.Filters;
using VIOBANK.Domain.Models;
using VIOBANK.Domain.Stores;
using Microsoft.EntityFrameworkCore;
using VIOBANK.Domain.Enums;
using System.Data;

namespace VIOBANK.PostgresPersistence.Repositories
{
    public class UsersRepository : IUserStore
    {
        private readonly VIOBANKDbContext _context;

        public UsersRepository(VIOBANKDbContext context)
        {
            _context = context;
        }

        public async Task Add(User user)
        {
            var userRole = await _context.Roles
                .SingleOrDefaultAsync(r => r.Id == (int)RoleEnum.User)
                ?? throw new InvalidOperationException("Role 'User' not found.");

            user.Roles = new List<Role> { userRole };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IReadOnlyList<User>> GetByFilter(UserFilter filter)
        {
            var query = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Email))
                query = query.Where(u => u.Email.Contains(filter.Email));

            if (!string.IsNullOrWhiteSpace(filter.Name))
                query = query.Where(u => u.Name.Contains(filter.Name));

            if (!string.IsNullOrWhiteSpace(filter.Surname))
                query = query.Where(u => u.Surname.Contains(filter.Surname));

            if (!string.IsNullOrWhiteSpace(filter.Phone))
                query = query.Where(u => u.Phone.Contains(filter.Phone));

            if (!string.IsNullOrWhiteSpace(filter.IdCard))
                query = query.Where(u => u.IdCard.Contains(filter.IdCard));

            if (!string.IsNullOrWhiteSpace(filter.TaxNumber))
                query = query.Where(u => u.TaxNumber.Contains(filter.TaxNumber));

            if (filter.CreatedAfter.HasValue)
                query = query.Where(u => u.CreatedAt >= filter.CreatedAfter.Value);

            if (filter.CreatedBefore.HasValue)
                query = query.Where(u => u.CreatedAt <= filter.CreatedBefore.Value);

            return await query.ToListAsync();
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User> GetByCardNumber(string cardNumber)
        {
            var card = await _context.Cards
                .Include(c => c.Account)
                .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);

            return card?.Account?.User;
        }

        public async Task Update(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }


        public async Task<HashSet<PermissionEnum>> GetUserPermissions(int userId)
        {
            var roles = await _context.Users.AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.UserId == userId)
                .Select(u => u.Roles)
                .ToArrayAsync();

            return roles
                .SelectMany(r => r)
                .SelectMany(r => r.Permissions)
                .Select(p => (PermissionEnum)p.Id)
                .ToHashSet();
        }

        public async Task<IReadOnlyList<User>> GetUsers()
        {
            return await _context.Users.AsNoTracking().Include(u => u.Roles).ToListAsync(); 
        }

        public async Task<List<PermissionEnum>> GetUserPermissionsList(int userId)
        {
            var roles = await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .ThenInclude(r => r.Permissions)
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.Roles)
                .ToListAsync();

            return roles
                .SelectMany(r => r.Permissions)
                .Select(p => (PermissionEnum)p.Id)
                .ToList();
        }

        public async Task<bool> ApplyRoleToUser(int userId, List<Role> roles)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return false;
            }

            var existingRoleIds = user.Roles.Select(r => r.Id).ToHashSet();

            foreach (var role in roles)
            {
                if (!existingRoleIds.Contains(role.Id))
                {
                    user.Roles.Add(role);
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Role>> ConvertToListRole(List<string> roleNames)
        {
            return await _context.Roles.AsNoTracking()
                .Where(r => roleNames.Contains(r.Name))
                .ToListAsync();
        }
    }
}
