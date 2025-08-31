namespace EasyDine.DTOs;

// DTO for API response wrapping

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    
    public static ApiResponse<T> Ok(T data, string message = "")
        => new() { Success = true, Data = data, Message = message };
    
    public static ApiResponse<T> Ok(string message = "")
        => new() { Success = true, Message = message };
    
    public static ApiResponse<T> Fail(string message)
        => new() { Success = false, Message = message };
}