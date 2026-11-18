using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KpiBackend.Data;
using KpiBackend.Models;
using KpiBackend.Dtos;
using KpiBackend.Dtos.Users;
using KpiBackend.Helpers;

namespace KpiBackend.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", async (RequestUserDto dto, KpiContext db) =>
        {
            if (string.IsNullOrWhiteSpace(dto.Name) || string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
                return Results.BadRequest(ApiResponse.Fail("Nama, Email, dan Password wajib diisi."));

            var isEmailExist = await db.Users.AnyAsync(u => u.Email == dto.Email);
            if (isEmailExist)
                return Results.BadRequest(ApiResponse.Fail("Email sudah terdaftar. Gunakan email lain."));

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var newUser = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = passwordHash, 
                Role = dto.Role,
                DepartmentId = dto.DepartmentId,
                SupervisorId = dto.SupervisorId,
                CreatedAt = DateTime.UtcNow
            };

            db.Users.Add(newUser);
            await db.SaveChangesAsync();

            var responseDto = new ResponseUserDto(
                newUser.Id, newUser.Name, newUser.Email, newUser.Role, newUser.DepartmentId, null, newUser.SupervisorId, null
            );

            return Results.Ok(ApiResponse.Ok(responseDto, "Registrasi berhasil."));
        });


        group.MapPost("/login", async (LoginRequestDto dto, KpiContext db, IConfiguration config) =>
        {
            var user = await db.Users
                .Include(u => u.Department)
                .Include(u => u.Supervisor)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Results.BadRequest(ApiResponse.Fail("Email atau Password salah."));
            }

            var token = GenerateJwtToken(user, config);

            var userDto = new ResponseUserDto(
                user.Id, user.Name, user.Email, user.Role, user.DepartmentId, user.Department?.Name, user.SupervisorId, user.Supervisor?.Name
            );
            
            var loginResponse = new LoginResponseDto(token, userDto);

            return Results.Ok(ApiResponse.Ok(loginResponse, "Login berhasil."));
        });
    }

    private static string GenerateJwtToken(User user, IConfiguration config)
    {
        var jwtSettings = config.GetSection("Jwt");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Role, user.Role) 
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"]!)),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = credentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return tokenHandler.WriteToken(token);
    }
}