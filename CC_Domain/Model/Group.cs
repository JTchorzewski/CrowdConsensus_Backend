namespace Domain.Model;

public class Group
{
    public int Id { get; set; }
    public string GroupName { get; set; }
    public ICollection<CompanyToGroupConnection> CompanyToGroupConnection { get; set; }
}