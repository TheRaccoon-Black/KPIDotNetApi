using System.ComponentModel.DataAnnotations;

namespace KpiBackend.Dtos.Departments;

public record RequestDepartmentDto(
    [Required] string Name,
    int? ManagerId
);
