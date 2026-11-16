using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace KpiBackend.Models;

public class KpiRealization
{
    public int Id {get; set;}
    public KpiAssignment? KpiAssignment {get;set;}
    public required int KpiAssignmentId {get; set;}
    public required decimal ActualValue { get; set;}
    public required decimal CalculatedScore { get; set;}
    public string? EvidenceUrl {get; set;}
    public string? EvaluatorNotes { get; set;}
    public int? EvaluatedBy { get; set; }

    [ForeignKey("EvaluatedBy")]
    public  User? Evaluator {get;set;}
    public DateTime? EvaluatedAt { get; set; }
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set;}

}
