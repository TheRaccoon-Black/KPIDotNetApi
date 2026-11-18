using System.ComponentModel.DataAnnotations.Schema;

namespace KpiBackend.Models;

public class User
{
    public int Id {get; set;}

    public required string Email {get; set;}
    public required string Name { get; set;}

    public required string PasswordHash { get; set; }
    public Department? Department {get; set;}
    [ForeignKey("DepartmentId")]
    public int? DepartmentId {get; set;}
    public int? SupervisorId { get; set;}
    
    [ForeignKey("SupervisorId")]
    public  User? Supervisor {get; set;}
    public string Role {get; set;} = "employee";
    public DateTime? CreatedAt {get; set;}
    public DateTime? UpdatedAt {get ; set;}
    public DateTime? DeletedAt{get; set;}

    

}
