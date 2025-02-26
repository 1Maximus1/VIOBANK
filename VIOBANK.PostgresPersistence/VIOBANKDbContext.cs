
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VIOBANK.Domain.Models;
using VIOBANK.PostgresPersistence.AuthOptions;
using VIOBANK.PostgresPersistence.Configurations;
using VIOBANK.PostgresPersistence.Entities;

namespace VIOBANK.PostgresPersistence
{
    public class VIOBANKDbContext: DbContext
    {
        private readonly IOptions<AuthorizationOptions> _authOptions;
        public VIOBANKDbContext(DbContextOptions<VIOBANKDbContext> options, IOptions<AuthorizationOptions> authOptions) : base(options)
        {
            _authOptions = authOptions;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermissionEnity> RolePermissions { get; set; }
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<MobileTopup> MobileTopups { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<DepositTransaction> DepositTransactions { get; set; }
        public DbSet<WithdrawnDeposit> WithdrawnDeposits { get; set; }
        public DbSet<BlacklistedToken> BlacklistedTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CardConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new DepositConfiguration());
            modelBuilder.ApplyConfiguration(new MobileTopupConfiguration());
            modelBuilder.ApplyConfiguration(new SettingsConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new DepositTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new WithdrawnDepositConfiguration());
            modelBuilder.ApplyConfiguration(new BlacklistedTokenConfiguration());
            
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration(_authOptions.Value));
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
