using HR4you.Security.Contexts;
using HR4you.Security.Handler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace HR4you.Security.Controllers;

[ApiController]
[Route("/api/auth/")]
public class UserController : ControllerBase
{
    private readonly UserContext _db;
    private readonly PasswordHandler _passwordHandler;

    public UserController(UserContext db, PasswordHandler passwordHandler)
    {
        _db = db;
        _passwordHandler = passwordHandler;
    }

    [HttpPost("add-new-user")]
    [SwaggerOperation("AddNewUser")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult AddNewUser([FromBody] AddUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.UserName) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("user name / password must not be empty");
        }

        var user = _db.GetUserByName(request.UserName);

        if (user != null)
        {
            return BadRequest("user already exists");
        }

        _db.CreateUser(request.UserName, request.FirstName, request.LastName, request.Password,
            request.RoleGroups);

        return Ok();
    }


    [HttpPost("change-user-data")]
    [SwaggerOperation("ChangeUserData")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult ChangeUserData([FromBody] ChangeUserRequest request)
    {
        var user = _db.GetUserById(request.Id);

        if (user == null)
        {
            return BadRequest("unknown user");
        }
        
        var userName = HttpContext.User.Identity!.Name!;
        if (user.UserName == userName)
        {
            return BadRequest("user cannot change himself!");
        }

        if (request.UserName != null)
        {
            user.UserName = request.UserName;
        }

        if (request.FirstName != null)
        {
            user.FirstName = request.FirstName;
        }

        if (request.LastName != null)
        {
            user.LastName = request.LastName;
        }

        _db.SetUserRoles(user, request.RoleGroups);

        return Ok();
    }


    [HttpPost("change-user-password")]
    [SwaggerOperation("ChangeUserPwd")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult ChangeUserPwd([FromBody] ChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewPw))
        {
            return BadRequest("new pwd must not be empty");
        }
        
        if (string.IsNullOrWhiteSpace(request.OldPw))
        {
            return BadRequest("old pwd must not be empty");
        }

        var user = _db.GetUserById(request.Id);

        if (user == null)
        {
            return BadRequest("unknown user");
        }

        var pwdOk = _passwordHandler.IsPasswordOk(request.OldPw, user.PasswordHash!);

        if (!pwdOk)
        {
            return BadRequest("wrong password");
        }


        if (string.Equals(request.NewPw, request.OldPw))
        {
            return BadRequest("new pwd = old pwd");
        }

        var newHash = _passwordHandler.HashPassword(request.NewPw);

        user.PasswordHash = newHash;

        _db.SaveChanges();
        return Ok();
    }

    [HttpPost("override-user-password")]
    [SwaggerOperation("OverrideUserPwd")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult OverrideUserPwd([FromBody] ChangePasswordRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.NewPw))
        {
            return BadRequest("new pwd must not be empty");
        }

        var user = _db.GetUserById(request.Id);

        if (user == null)
        {
            return BadRequest("unknown user");
        }

        var pwdOk = _passwordHandler.IsPasswordOk(request.NewPw, user.PasswordHash!);
        if (pwdOk)
        {
            return BadRequest("new pwd = old pwd");
        }

        var newHash = _passwordHandler.HashPassword(request.NewPw);

        user.PasswordHash = newHash;

        _db.SaveChanges();
        return Ok();
    }

    [HttpGet("get-all-users")]
    [SwaggerOperation("GetAllUsers")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult GetAllUsers(bool showDeleted = false)
    {
        var users = _db.GetUsers(showDeleted);
        var result = users.Select(user => UserInfo.FromUser(user, true)).ToList();

        return Ok(result);
    }
    
    [HttpGet("get-user")]
    [SwaggerOperation("GetUser")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult GetUser(string id)
    {
        var user = _db.GetUserById(id);
        if (user == null)
        {
            return BadRequest($"unknown user id - {id}");
        }
        
        var result = UserInfo.FromUser(user, true);
        
        return Ok(result);
    }


    [HttpPost("delete-user")]
    [SwaggerOperation("DeleteUser")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult DeleteUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("user id must not be empty");
        }

        var user = _db.GetUserById(id);
        if (user == null)
        {
            return BadRequest("user id unknown");
        }
        
        var userName = HttpContext.User.Identity!.Name!;
        if (user.UserName == userName)
        {
            return BadRequest("user cannot delete himself!");
        }


        if (user.IsDeleted)
        {
            return BadRequest("user already deleted");
        }

        user.IsDeleted = true;
        user.RoleGroups = [];
        _db.SaveChanges();

        return Ok();
    }


    [HttpPost("un-delete-user")]
    [SwaggerOperation("UnDeleteUser")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult UnDeleteUser(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            return BadRequest("user id must not be empty");
        }

        var user = _db.GetUserById(id);
        if (user == null)
        {
            return BadRequest("user id unknown");
        }

        if (!user.IsDeleted)
        {
            return BadRequest("user already un-deleted");
        }

        user.IsDeleted = false;
        _db.SaveChanges();

        return Ok();
    }

    [HttpPost("check-user-name")]
    [SwaggerOperation("CheckUserName")]
    //[Authorize(Policy = BuildInUserRoles.SysAdminRole)]
    public IActionResult CheckUserName(string userName)
    {
        var user = _db.GetUserByName(userName);

        return Ok(user == null);
    }
}

public class AddUserRequest
{
    public required string UserName { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Password { get; set; }
    public required string[] RoleGroups { get; set; }
}

public class ChangeUserRequest
{
    public required string Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string[] RoleGroups { get; set; }
}

public class ChangePasswordRequest
{
    public required string Id { get; set; }
    public string? OldPw { get; set; }
    public required string NewPw { get; set; }
}