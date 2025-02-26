using VIOBANK.Domain.Enums;
using VIOBANK.Domain.Filters;
using VIOBANK.Domain.Models;

namespace VIOBANK.Domain.Stores
{
    public interface IUserStore
    {
        Task<IReadOnlyList<User>> GetByFilter(UserFilter filter);
        Task<User> GetById(int id); 
        Task<User> GetByEmail(string email);
        Task<User> GetByCardNumber(string cardNumber);
        Task Add(User user);
        Task Update(User user);
        Task Delete(int id);
        Task<HashSet<PermissionEnum>> GetUserPermissions(int userId);
        Task<IReadOnlyList<User>> GetUsers();
        Task<List<PermissionEnum>> GetUserPermissionsList(int userId);
        Task<bool> ApplyRoleToUser(int userId, List<Role> roles);
        Task<List<Role>> ConvertToListRole(List<string> role);
    }
}
