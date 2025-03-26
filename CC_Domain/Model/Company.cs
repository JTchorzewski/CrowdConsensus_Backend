namespace Domain.Model;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NextRaportDate { get; set; }
    public ICollection<CompanyToGroupConnection> CompanyToGroupConnection { get; set; }
}