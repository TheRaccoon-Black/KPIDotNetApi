using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;

namespace KpiBackend.Models;

public class KpiAssignment
{
    public int Id {get; set;}
    public User? User {get;set;}
    [ForeignKey("UserId")]
    public required int UserId{get; set;}
    public required int PeriodId{ get; set;}
    [ForeignKey("PeriodId")]
    public KpiPeriod? Period {get; set;}
    public required int MasterId { get; set;}
    [ForeignKey("MasterId")]
    public KpiMaster? Master {get; set;}
    public required decimal WeightPercentage { get; set;}
    public required decimal TargetValue { get; set;}
    public string Status {get; set;} = "draft";
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set;}
    

}
