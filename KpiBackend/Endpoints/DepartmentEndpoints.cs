
using Azure.Core;
using KpiBackend.Data;
using KpiBackend.Dtos.Departments;
using KpiBackend.Models;
using Microsoft.EntityFrameworkCore;
using KpiBackend.Helpers;

namespace KpiBackend.Endpoints;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoints(this IEndpointRouteBuilder app)
    {
            var group = app.MapGroup("/api/departments").WithTags("Departments");
            group.MapGet("/", async (KpiContext db) =>
            {
                var departments = await db.Departments
                .Include(d=>d.Manager)
                .Select(d=> new ResponseDepartmentDto(
                    d.Id,
                    d.Name,
                    d.ManagerId,
                    d.Manager != null? d.Manager.Name : null
                ))
                .ToListAsync();

                // return Results.Ok(departments);
                return Results.Ok(ApiResponse.Ok(departments));
            });

            group.MapGet("/{id}",async (int id, KpiContext db) =>
            {
                var department = await db.Departments
                .Include(d=>d.Manager)
                .FirstOrDefaultAsync(d =>d.Id == id);

                if (department is null)
                // return Results.NotFound(new { message = $"Department dengan id {id} tidak ditemukan"});
                    return Results.NotFound(ApiResponse.Fail($"Departemen dengan Id {id} tidak ditemukan."));

                var response = new ResponseDepartmentDto(
                    department.Id,
                    department.Name,
                    department.ManagerId,
                    department.Manager?.Name
                );

                // return Results.Ok(response);
                return Results.Ok(ApiResponse.Ok(response, "Detail departemen berhasil diambil."));
        
            });

            group.MapPost("/", async(RequestDepartmentDto dto, KpiContext db) =>
            {

                var now = DateTime.UtcNow;
                if (string.IsNullOrWhiteSpace(dto.Name))
                    // return Results.BadRequest(ApiResponse.Fail("Nama departemen wajib diisi."));
                    return Results.BadRequest(ApiResponse.Fail("Nama departemen wajib diisi."));
                var newDepartment = new Department
                {
                    Name = dto.Name,
                    ManagerId = dto.ManagerId,
                    CreatedAt = now
                };

                db.Departments.Add(newDepartment);
                await db.SaveChangesAsync();

                var response = new ResponseDepartmentDto(
                    newDepartment.Id,
                    newDepartment.Name,
                    newDepartment.ManagerId,
                    null
                );
                return Results.Created($"/api/departments/{newDepartment.Id}", 
                ApiResponse.Ok(response, "Departemen baru berhasil ditambahkan."));
            });
    }
}
