namespace PSite3.Services.Common;

public sealed class ServiceResult<T>
{
    public ServiceStatus Status { get; private init; }
    public T? Value { get; private init; }
    public string? Error { get; private init; }

    public bool IsSuccess => Status is ServiceStatus.Ok
        or ServiceStatus.Created
        or ServiceStatus.NoContent;

    public static ServiceResult<T> Ok(T value)
        => new() { Status = ServiceStatus.Ok, Value = value };

    public static ServiceResult<T> Created(T value)
        => new() { Status = ServiceStatus.Created, Value = value };

    public static ServiceResult<T> NoContent()
        => new() { Status = ServiceStatus.NoContent };

    public static ServiceResult<T> NotFound(string error)
        => new() { Status = ServiceStatus.NotFound, Error = error };

    public static ServiceResult<T> Validation(string error)
        => new() { Status = ServiceStatus.ValidationError, Error = error };

    public static ServiceResult<T> Conflict(string error)
        => new() { Status = ServiceStatus.Conflict, Error = error };
}
