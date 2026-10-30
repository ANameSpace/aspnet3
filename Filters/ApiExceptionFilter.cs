using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace PSite3.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
        {
            return;
        }

        var (statusCode, title, detail) = context.Exception switch
        {
            DbUpdateConcurrencyException => (
                StatusCodes.Status409Conflict,
                "Конфликт",
                "Данные были изменены другим запросом. Повторите операцию."),
            DbUpdateException => (
                StatusCodes.Status409Conflict,
                "Конфликт",
                "Операция нарушает ограничения базы данных."),
            _ => (
                StatusCodes.Status500InternalServerError,
                "Внутренняя ошибка сервера",
                "Произошла непредвиденная ошибка.")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
        {
            _logger.LogError(context.Exception, "Необработанная ошибка API для {Path}", context.HttpContext.Request.Path);
        }
        else
        {
            _logger.LogWarning(context.Exception, "Конфликт данных API для {Path}", context.HttpContext.Request.Path);
        }

        context.Result = new ObjectResult(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = context.HttpContext.Request.Path
        })
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}
