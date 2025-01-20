using System.Text;
using HR4you.Security.Contexts;
using HR4you.Security.Handler;
using HR4you.Security.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HR4you.Security.Authentication;

public static class CustomAuthentication
{
    public static void AddCustomAuthentication(this IServiceCollection services, AuthenticationConfig config)
    {
        if (string.IsNullOrWhiteSpace(config.JwtKey) || string.IsNullOrWhiteSpace(config.JwtIssuer))
        {
            throw new ArgumentException("Jwt:Key and Jwt:Issuer must not be null nor empty");
        }

        services.AddTransient<PasswordHandler>();
        services.AddTransient<JwtTokenHandler>();
        services.AddTransient<JwtBlacklistHandler>();
        services.AddTransient<JwtBlackListMiddleware>();

        services.AddDbContext<UserContext>(b =>
            b.UseMySql(config.ConnectionString, ServerVersion.AutoDetect(config.ConnectionString)));

        services.AddIdentityCore<User>().AddEntityFrameworkStores<UserContext>();
        services.AddAuthentication()
            .AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config.JwtIssuer,
                    ValidAudience = config.JwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.JwtKey)),
                    ClockSkew = TimeSpan.Zero
                })
            .AddNegotiate();

        var sp = services.BuildServiceProvider();
        var uc = sp.GetService<UserContext>()!;
        uc.Database.Migrate();

        if (config.DomainUserConfig != null)
        {
            foreach (var domainUserRole in config.DomainUserConfig.DomainUserRoles.Where(domainUserRole => uc.Roles
                         .Where(ur => ur.Name == domainUserRole.Name)
                         .AsEnumerable()
                         .FirstOrDefault(ur => ur.Name == domainUserRole.Name) == null))
            {
                uc.Roles.AddRange(domainUserRole);
            }

            uc.SaveChanges();
            foreach (var userRoleGroup in config.DomainUserConfig.DomainUserRoleGrpups.Where(userRoleGroup => uc
                         .UserRoleGroups.Where(ur => ur.Name == userRoleGroup.Name)
                         .AsEnumerable()
                         .FirstOrDefault(ur => ur.Name == userRoleGroup.Name) == null))
            {
                uc.UserRoleGroups.AddRange(userRoleGroup);
            }

            uc.SaveChanges();
            foreach (var user in config.DomainUserConfig.DomainUsers)
            {
                uc.CreateUser(user.UserName, string.Empty, string.Empty, user.Password, user.RoleGroups);
            }
        }

        var roles = BuildInUserRoles.BuildInUserRoleList;
        
        services.AddAuthorization(options =>
        {
            foreach (var userRole in roles)
            {
                AuthorizationPolicyBuilder authorizationPolicyBuilder;
                switch (userRole.AuthScheme)
                {
                    case JwtBearerDefaults.AuthenticationScheme:
                        authorizationPolicyBuilder = new AuthorizationPolicyBuilder(userRole.AuthScheme);
                        options.AddPolicy(userRole.Name, authorizationPolicyBuilder
                            .RequireAuthenticatedUser()
                            .RequireRole(userRole.Name)
                            .Build());
                        break;
                    case NegotiateDefaults.AuthenticationScheme:
                        authorizationPolicyBuilder = new AuthorizationPolicyBuilder(userRole.AuthScheme);
                        options.AddPolicy(userRole.Name, authorizationPolicyBuilder
                            .RequireAuthenticatedUser()
                            .Build());
                        break;
                }
            }
        });
    }

    public static void UseCustomAuthentication(this IApplicationBuilder builder)
    {
        builder.UseAuthentication();
        builder.UseAuthorization();

        builder.UseMiddleware<JwtBlackListMiddleware>();
    }
}