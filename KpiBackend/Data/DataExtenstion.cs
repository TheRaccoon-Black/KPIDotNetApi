using System;
using Microsoft.EntityFrameworkCore;

namespace KpiBackend.Data;

public static class DataExtenstion
{
    public static void AddKpiContext(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Daftarkan AppDbContext menggunakan SQL Server
builder.Services.AddDbContext<KpiContext>(options =>
{
    options.UseSqlServer(connectionString);
    
    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.LogTo(Console.WriteLine, LogLevel.Information);
    }
});
    }
}
