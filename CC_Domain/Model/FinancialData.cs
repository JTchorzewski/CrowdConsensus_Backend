using System.Text.Json.Serialization;

namespace Domain.Model;

public class FinancialData
{
    [JsonIgnore]
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public decimal NetProfit { get; set; }
    public decimal Revenue { get; set; }
    public string RaportDate { get; set; }
    public Company Company { get; set; }
}