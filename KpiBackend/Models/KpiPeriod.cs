using System;

namespace KpiBackend.Models;

public class KpiPeriod
{
    public int Id {get; set;}
    public required string Name { get; set;}
    public required DateTime StartDate { get ; set;}
    public required DateTime EndDate { get ; set;}
    public string Status {get; set;} = "planning";
    public DateTime? CreatedAt{get; set;}
    public DateTime? UpdatedAt{get; set;}
}
