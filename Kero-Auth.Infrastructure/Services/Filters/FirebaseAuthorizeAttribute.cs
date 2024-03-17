using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

public class FirebaseAuthorizationFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string authorizationHeader = null;

        // Check for the Authorization header
        if (context.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            authorizationHeader = context.HttpContext.Request.Headers["Authorization"];
        }
        // If Authorization header is not present, check for X-Forwarded-Authorization header
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
