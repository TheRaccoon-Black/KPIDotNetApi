using System.ComponentModel.DataAnnotations;

namespace KpiBackend.Dtos.Departments;

public record RequestDepartmentDto(
    [Required(ErrorMessage ="Name can not be null")] string Name,
    int? ManagerId
);
