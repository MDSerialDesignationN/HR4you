using System.Net;
using HR4you.Security.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace HR4you.Security.Authentication;

public class JwtBlackListMiddleware : IMiddleware
{
    private readonly JwtBlacklistHandler _blackList;

    public JwtBlackListMiddleware(JwtBlacklistHandler blackList)
    {
        _blackList = blackList;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var authorizationHeader = context.Request.Headers.Authorization;
 
        var token =  authorizationHeader == StringValues.Empty
            ? string.Empty
            : authorizationHeader.Single()!.Split(" ").Last();
        
        if (_blackList.IsTokenOk(token))
        {
            await next(context);
            
            return;
        }
        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
    }
}