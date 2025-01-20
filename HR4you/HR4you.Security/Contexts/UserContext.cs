using HR4you.Security.Handler;
using HR4you.Security.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Design;

namespace HR4you.Security.Contexts;

public class UserContext : DbContext
{
    private readonly PasswordHandler _passwordHandler;

    public UserContext(DbContextOptions<UserContext> options, PasswordHandler passwordHandler)
        : base(options)
    {
        _passwordHandler = passwordHandler;
    }

    private DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> Roles { get; set; } = null!;
    public DbSet<BlacklistedToken> TokenBlacklist { get; set; } = null!;
    public DbSet<UserRoleGroup> UserRoleGroups { get; set; } = null!;
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var valueComparer = new ValueComparer<string[]>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c);

        modelBuilder.Entity<User>()
            .HasMany(e => e.RoleGroups)
            .WithMany(e => e.Users)
            .UsingEntity("hr4you_userToUserRoleGroups");

        modelBuilder
            .Entity<UserRoleGroup>()
            .Property(e => e.Roles)
            .HasConversion(
                v => v == null ? null : string.Join(',', v),
                v => v == null ? null : v.Split(',', StringSplitOptions.RemoveEmptyEntries));

        modelBuilder
            .Entity<UserRoleGroup>()
            .Property(e => e.Roles)
            .Metadata
            .SetValueComparer(valueComparer);
    }

    public List<UserRoleGroup>? GetUserRoleGroupsByName(string[] requestRolesGroups)
    {
        var result = new List<UserRoleGroup>();
        foreach (var rolesGroup in requestRolesGroups)
        {
            var userRoleGroup = UserRoleGroups.FirstOrDefault(g => g.Name == rolesGroup);
            if (userRoleGroup == null)
            {
                return null;
            }
            result.Add(userRoleGroup);
        }
        return result;
    }
    

    public User? GetUserByName(string username)
    {
        return Users.Include(e=>e.RoleGroups).Where(x => x.UserName==username)
            .AsEnumerable()
            .FirstOrDefault(t => t.UserName == username);
    }

    public List<User> GetUsers(bool showDeleted)
    {
        return showDeleted ? 
            Users.Include(e=>e.RoleGroups).ToList() : 
            Users.Include(e=>e.RoleGroups).Where(u => u.IsDeleted == false).ToList();

    }

    public void AddUser(User newUser)
    {
        Users.Add(newUser);
        SaveChanges();
    }

    public User? GetUserById(string id)
    {
        return Users.Include(u=>u.RoleGroups).FirstOrDefault(u=>u.Id == id);
    }

    public void SetUserRoles(User user, string[] requestRolesGroups)
    {
        var groups = GetUserRoleGroupsByName(requestRolesGroups);
        user.RoleGroups.RemoveAll(_=>true);
        user.RoleGroups = groups;
        SaveChanges();
    }

    public void CreateUser(string userName, string firstName, string lastName, string password, string[] roleGroups)
    {
        var user = GetUserByName(userName);
        if (user != null) return;
        var hash = _passwordHandler.HashPassword(password);
        var roleGroupsDb = GetUserRoleGroupsByName(roleGroups);
        var newUser = new User
        {
            UserName = userName,
            PasswordHash = hash,
            FirstName = firstName,
            LastName = lastName,
            RoleGroups = roleGroupsDb
        };
        AddUser(newUser);
    }
    
    public class UserContextDesignTimeFactory : IDesignTimeDbContextFactory<UserContext>
    {
        public UserContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<UserContext>();

            builder.UseMySql(args[0], ServerVersion.AutoDetect(args[0]));
            return new UserContext(builder.Options, null!);
        }
    }
}