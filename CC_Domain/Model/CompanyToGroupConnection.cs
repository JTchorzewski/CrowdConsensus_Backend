namespace Domain.Model;

public class CompanyToGroupConnection
{
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
}