namespace Kero_Auth.Middlewares;

using Firebase.Auth;
using FirebaseAdmin.Auth;
using Kero_Auth.Domain.Authentication.Exceptions;
using Kero_Auth.Domain.SharedKernel;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class FirebaseAuthExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<FirebaseAuthExceptionHandlerMiddleware> _logger;

    public FirebaseAuthExceptionHandlerMiddleware(RequestDelegate next, ILogger<FirebaseAuthExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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
        catch (BusinessException ex)
        {
            await HandleBusinessExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private Task HandleFirebaseAuthExceptionAsync(HttpContext context, FirebaseAuthHttpException ex)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errorMessage) = ex.Reason switch
        {
            AuthErrorReason.UserDisabled => ((int)HttpStatusCode.Forbidden, "The user account has been disabled."),
            AuthErrorReason.UserNotFound => ((int)HttpStatusCode.NotFound, "Kero not found the user on the system."),
            AuthErrorReason.InvalidEmailAddress => ((int)HttpStatusCode.BadRequest, "Invalid email address format."),
            AuthErrorReason.WrongPassword => ((int)HttpStatusCode.Unauthorized, "Incorrect password."),
            AuthErrorReason.TooManyAttemptsTryLater => ((int)HttpStatusCode.TooManyRequests, "Kero detect too many attempts, try again later."),
            AuthErrorReason.WeakPassword => ((int)HttpStatusCode.BadRequest, "Kero don't accept weak passwords use an stronger password."),
            AuthErrorReason.EmailExists => ((int)HttpStatusCode.BadRequest, "Kero already have that email on the system."),
            AuthErrorReason.OperationNotAllowed => ((int)HttpStatusCode.MethodNotAllowed, "Kero don't allow that operation."),
            AuthErrorReason.MissingEmail => ((int)HttpStatusCode.BadRequest, "Kero needs the email."),
            AuthErrorReason.UnknownEmailAddress => ((int)HttpStatusCode.NotFound, "Kero not found the user on the system."),
            AuthErrorReason.AccountExistsWithDifferentCredential => ((int)HttpStatusCode.Conflict, "Kero detect that an account already exists with the same email address."),
            AuthErrorReason.Unknown => ((int)HttpStatusCode.InternalServerError, "An unknown error occurred."),
            _ => ((int)HttpStatusCode.InternalServerError, "An unexpected error occurred.")
        };

        context.Response.StatusCode = statusCode;
        var result = JsonSerializer.Serialize(new { message = errorMessage });
        return context.Response.WriteAsync(result);
    }

    private Task HandleBusinessExceptionAsync(HttpContext context, BusinessException ex)
    {
        context.Response.ContentType = "application/json";
        int statusCode;
        string errorMessage;

        switch (ex)
        {
            case UserNotFoundException _:
                statusCode = (int)HttpStatusCode.NotFound;
                errorMessage = "Kero not found the user on the system.";
                break;
            default:
                statusCode = (int)HttpStatusCode.BadRequest;
                errorMessage = "A business error occurred.";
                break;
        }

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
