namespace Kero_Auth.Infrastructure.Services.Filters;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using FirebaseAdmin.Auth;
using System.Threading.Tasks;

public class KeroAuthorizeAttribute : TypeFilterAttribute
{
    public KeroAuthorizeAttribute() : base(typeof(FirebaseAuthorizationFilter))
    {
    }
}

public class FirebaseAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string authorizationHeader = string.Empty;

        if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
        }
        else if (context.HttpContext.Request.Headers.ContainsKey("X-Forwarded-Authorization"))
        {
            authorizationHeader = context.HttpContext.Request.Headers["X-Forwarded-Authorization"];
        }

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        string idToken = authorizationHeader.Substring("Bearer ".Length);

        try
        {
            FirebaseToken decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken);
        }
        catch (FirebaseAuthException)
        {
            context.Result = new UnauthorizedResult();
            return;
        }
    }
}
