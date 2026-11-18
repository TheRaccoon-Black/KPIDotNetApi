namespace KpiBackend.Helpers;

public class Meta
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
}

// 1. CLASS UTAMA DENGAN GENERIC <T> (Kode Anda sebelumnya, tetap biarkan)
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public object? Errors { get; set; }
    public Meta? Meta { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    public static ApiResponse<T> Ok(string message = "Success")
    {
        return new ApiResponse<T> { Success = true, Message = message };
    }

    public static ApiResponse<T> Paginated(T data, Meta meta, string message = "Success")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data, Meta = meta };
    }

    public static ApiResponse<T> Fail(string message, object? errors = null)
    {
        return new ApiResponse<T> { Success = false, Message = message, Errors = errors };
    }
}

public static class ApiResponse
{
    public static ApiResponse<T> Ok<T>(T data, string message = "Success")
    {
        return ApiResponse<T>.Ok(data, message);
    }

    public static ApiResponse<object> Ok(string message = "Success")
    {
        return ApiResponse<object>.Ok(message);
    }

    public static ApiResponse<T> Paginated<T>(T data, Meta meta, string message = "Success")
    {
        return ApiResponse<T>.Paginated(data, meta, message);
    }

    public static ApiResponse<object> Fail(string message, object? errors = null)
    {
        return ApiResponse<object>.Fail(message, errors);
    }
}