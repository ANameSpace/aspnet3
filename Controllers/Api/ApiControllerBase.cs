using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PSite3.Filters;
using PSite3.Services.Common;

namespace PSite3.Controllers.Api;

[TypeFilter(typeof(ApiExceptionFilter))]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> FromResult<T>(ServiceResult<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Status == ServiceStatus.NoContent
                ? NoContent()
                : Ok(result.Value);
        }

        return ProblemResult(result.Status, result.Error);
    }

    protected ActionResult<IEnumerable<T>> FromPagedResult<T>(
        ServiceResult<PagedResult<T>> result)
    {
        if (!result.IsSuccess || result.Value is null)
        {
            return ProblemResult(result.Status, result.Error);
        }

        Response.Headers["X-Total-Count"] = result.Value.TotalCount.ToString(CultureInfo.InvariantCulture);

        return Ok(result.Value.Items);
    }

    protected ActionResult<T> CreatedFromResult<T>(
        ServiceResult<T> result,
        string actionName,
        Func<T, int> idSelector)
    {
        if (result.Status == ServiceStatus.Created && result.Value is not null)
        {
            return CreatedAtAction(actionName, new { id = idSelector(result.Value) }, result.Value);
        }

        return FromResult(result);
    }

    private static ObjectResult ProblemResult(ServiceStatus status, string? error)
    {
        var (statusCode, title) = status switch
        {
            ServiceStatus.NotFound => (StatusCodes.Status404NotFound, "Не найдено"),
            ServiceStatus.ValidationError => (StatusCodes.Status400BadRequest, "Некорректный запрос"),
            ServiceStatus.Conflict => (StatusCodes.Status409Conflict, "Конфликт"),
            _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера")
        };

        return new ObjectResult(new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = error
        })
        {
            StatusCode = statusCode
        };
    }
}
