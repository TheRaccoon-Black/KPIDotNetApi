using System.ComponentModel.DataAnnotations;
using Microsoft.Identity.Client;

namespace KpiBackend.Dtos.Departments;

public record ResponseDepartmentDto(
    int Id,
    string Name,
    int? ManagerId,
    string? ManagerName
);
