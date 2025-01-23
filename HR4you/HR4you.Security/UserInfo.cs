using HR4you.Security.Models;
using Newtonsoft.Json;

namespace HR4you.Security;

public class UserInfo
{
    public string Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string AuthToken { get; set; }
    public bool Deleted { get; set; }
    public string[] Roles { get; set; } = [];
    public string[] RoleGroups { get; set; } = [];

    public static UserInfo FromUser(User user, bool omitToken)
    {
        return new UserInfo
        {
            Id = user.Id,
            UserName = user.UserName,
            AuthToken = omitToken ? string.Empty : user.AuthToken!,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Deleted = user.IsDeleted,
            Roles = user.GetRoles().ToArray(),
            RoleGroups = user.RoleGroups != null
                ? user.RoleGroups.Select(g => g.Name).ToArray()
                : []
        };
    }
}

//TODO - Yes, this uses Newtonsoft. Am I happy about it? Fuck no! Does it fucking work? Yes!
public static class UserInfoSerializer
{
    public static UserInfo FromJson(this string json)
    {
        return JsonConvert.DeserializeObject<UserInfo>(json)!;
    }

    public static string ToJson(this UserInfo userInfo)
    {
        return JsonConvert.SerializeObject(userInfo);
    }
}