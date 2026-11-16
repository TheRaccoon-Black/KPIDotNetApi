using System;
using Microsoft.Identity.Client;

namespace KpiBackend.Models;

public class KpiMaster
{
    public int Id {get;set;}
    public required string Code { get; set;}
    public required string Name {get;set;}
    public  required string UnitOfMeasurement { get; set;}
    public string Polarity { get; set;} = "positive";

    public DateTime? CreatedAt{get;set;}
    public DateTime? UpdatedAt{get; set;}
}
