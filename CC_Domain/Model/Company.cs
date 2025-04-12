namespace Domain.Model;

public class Company
{
    public int Id { get; set; }
    public string Symbol { get; set; }
    public string Name { get; set; }
    public ICollection<Quotation> Quotations { get; set; }
}