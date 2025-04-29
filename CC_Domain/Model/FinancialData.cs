namespace Domain.Model;

public class FinancialData
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public decimal NetProfit { get; set; }
    public decimal Revenue { get; set; }
    public DateTime RaportDate { get; set; }
}