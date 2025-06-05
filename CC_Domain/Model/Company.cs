namespace Domain.Model;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<FinancialData> FinancialData { get; set; }
    public List<Estimate> Estimate { get; set; }
}