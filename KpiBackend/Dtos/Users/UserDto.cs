using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace KpiBackend.Dtos.Users;

public record RequestUserDto(
    [Required] string Name,
    [Required, EmailAddress] string Email,
    [Required, MinLength(06)] string Password,
    [Required] string Role,
    int? DepartmentId,
    int? SupervisorId
);

public record ResponseUserDto(
    int Id,
    string Name,
    string Email,
    string Role,
    int? DepartmentId,
    string? DepartmentName,
    int? SupervisorId,
    string? SupervisorName
    
);


public record LoginRequestDto(
    [Required, EmailAddress] string Email,
    [Required] string Password
);

public record LoginResponseDto(
    string Token,
    ResponseUserDto User
);