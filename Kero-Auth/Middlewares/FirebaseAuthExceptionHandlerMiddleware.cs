namespace Kero_Auth.Middlewares;

using Firebase.Auth;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class FirebaseAuthExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public FirebaseAuthExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (FirebaseAuthHttpException ex)
        {
            await HandleFirebaseAuthExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private Task HandleFirebaseAuthExceptionAsync(HttpContext context, FirebaseAuthHttpException ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorMessage) = ex.Reason switch
        {
            AuthErrorReason.UserDisabled => ((int)HttpStatusCode.Forbidden, "The user account has been disabled."),
            AuthErrorReason.UserNotFound => ((int)HttpStatusCode.NotFound, "User not found."),
            AuthErrorReason.InvalidEmailAddress => ((int)HttpStatusCode.BadRequest, "Invalid email address format."),
            AuthErrorReason.WrongPassword => ((int)HttpStatusCode.Unauthorized, "Incorrect password."),
            AuthErrorReason.TooManyAttemptsTryLater => ((int)HttpStatusCode.TooManyRequests, "Too many attempts, try again later."),
            AuthErrorReason.WeakPassword => ((int)HttpStatusCode.BadRequest, "The password is too weak."),
            AuthErrorReason.EmailExists => ((int)HttpStatusCode.BadRequest, "Email already exists."),
            AuthErrorReason.OperationNotAllowed => ((int)HttpStatusCode.MethodNotAllowed, "Operation is not allowed."),
            AuthErrorReason.MissingEmail => ((int)HttpStatusCode.BadRequest, "Missing email."),
            AuthErrorReason.UnknownEmailAddress => ((int)HttpStatusCode.NotFound, "Email address not found."),
            AuthErrorReason.AccountExistsWithDifferentCredential => ((int)HttpStatusCode.Conflict, "An account already exists with the same email address but different sign-in credentials."),
            AuthErrorReason.Unknown => ((int)HttpStatusCode.InternalServerError, "An unknown error occurred."),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { message = errorMessage });
        return context.Response.WriteAsync(result);
    }

    private Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "application/json";
        var result = JsonSerializer.Serialize(new { detail = "An error occurred processing your request." });
        return context.Response.WriteAsync(result);
    }
}
