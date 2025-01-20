using HR4you.Security.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HR4you.Security;

public class DefaultUsersAndGroups
{
    public List<UserRole> DomainUserRoles { get; set; } = BuildInUserRoles.BuildInUserRoleList;
    public List<UserRoleGroup> DomainUserRoleGrpups { get; set; } = BuildInUserRoleGroups.BuildInUserRoleGroupList;
    public List<DefaultUser> DomainUsers { get; set; } = BuildInUsers.BuildInUsersList;
}

    
public static class BuildInUserRoles
{
    public const string Authenticated = "UserRole_Authenticated"; // has token but roles are not relevant
    
    // roles to check the token against
    public const string AdminRole = "UserRole_Admin";
    public const string SysAdminRole = "UserRole_SysManagement";
    
    public static readonly List<UserRole> BuildInUserRoleList =
    [
        new UserRole
        {
            Name = Authenticated,
            AuthScheme = JwtBearerDefaults.AuthenticationScheme,
            IsSpecialRole = true
        },
        new UserRole
        {
            Name = AdminRole,
            AuthScheme = JwtBearerDefaults.AuthenticationScheme
        },
        new UserRole
        {
            Name = SysAdminRole,
            AuthScheme = JwtBearerDefaults.AuthenticationScheme
        }
    ];
}

public static class BuildInUserRoleGroups
    {
        public const string SysAdmin = "HR_UserRoleGroup_SysAdmin";
        public const string Admin = "HR_UserRoleGroup_Admin";
        public const string Authenticated = "HR_UserRoleGroup_Authenticated";

        public static readonly List<UserRoleGroup> BuildInUserRoleGroupList =
        [
            new UserRoleGroup
            {
                Name = SysAdmin,
                Roles =
                [
                    BuildInUserRoles.SysAdminRole,
                    BuildInUserRoles.AdminRole,
                    BuildInUserRoles.Authenticated
                ]
            },
            new UserRoleGroup
            {
                Name = Admin,
                Roles =
                [
                    BuildInUserRoles.AdminRole,
                    BuildInUserRoles.Authenticated
                ]
            },
            new UserRoleGroup
            {
                Name = Authenticated,
                Roles =
                [
                    BuildInUserRoles.Authenticated
                ]
            }
        ];
    }

public static class BuildInUsers
{
    public const string SysAdmin = "sysAdmin";
    public const string Admin = "admin";
    public const string AuthenticatedUser = "user";

    public static readonly List<DefaultUser> BuildInUsersList =
    [
        new()
        {
            UserName = SysAdmin,
            Password = SysAdmin,
            RoleGroups =
            [
                BuildInUserRoleGroups.SysAdmin
            ]
        },

        new()
        {
            UserName = Admin,
            Password = Admin,
            RoleGroups =
            [
                BuildInUserRoleGroups.Admin
            ]
        },

        new()
        {
            UserName = AuthenticatedUser,
            Password = AuthenticatedUser,
            RoleGroups =
            [
                BuildInUserRoleGroups.Authenticated
            ]
        }
    ];
}



public class DefaultUser
{
    public required string UserName  { get; set; }
    public required string Password  { get; set; }
    public string[] RoleGroups { get; set; } = [];
}