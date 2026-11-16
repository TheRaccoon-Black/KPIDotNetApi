using System.ComponentModel.DataAnnotations.Schema;

namespace KpiBackend.Models;

public class Department
{
    public int Id {get; set;}       
    public required string Name { get; set;}
    public int? ManagerId { get; set;}
    [ForeignKey("ManagerId")]
    public User? Manager { get; set;}
    public DateTime? CreatedAt { get; set;}
    public DateTime? UpdatedAt { get; set;}
}
