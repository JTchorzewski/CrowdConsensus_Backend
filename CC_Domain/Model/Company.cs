namespace Domain.Model;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<FinancialData> FinancialData { get; set; }
}