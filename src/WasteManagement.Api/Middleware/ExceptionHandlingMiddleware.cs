using System.Net;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WasteManagement.Application.Exceptions;

namespace WasteManagement.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
        catch (ValidationException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.BadRequest, ex.Message, errors: ex.Errors.Select(e => e.ErrorMessage));
        }
        catch (NotFoundException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ConflictException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (AppException ex)
        {
            await WriteProblemDetailsAsync(context, HttpStatusCode.BadRequest, ex.Message, ex.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao processar a requisição");
            await WriteProblemDetailsAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro interno. Tente novamente.");
        }
    }

    private static async Task WriteProblemDetailsAsync(HttpContext context, HttpStatusCode statusCode, string detail, string? code = null, IEnumerable<string>? errors = null)
    {
        if (context.Response.HasStarted)
        {
            return;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new ProblemDetails
        {
            Status = (int)statusCode,
            Detail = detail,
            Title = code ?? statusCode.ToString()
        };

        if (errors is not null)
        {
            problem.Extensions["errors"] = errors.ToArray();
        }

        await context.Response.WriteAsJsonAsync(problem);
    }
}
