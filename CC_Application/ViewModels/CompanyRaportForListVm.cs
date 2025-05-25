namespace Application.ViewModels;

public class CompanyRaportForListVm
{
    public int Id { get; set; }
    public string CompanyName { get; set; }
    public decimal NetProfit { get; set; }
    public decimal Revenue { get; set; }
    public string RaportDate { get; set; }
}