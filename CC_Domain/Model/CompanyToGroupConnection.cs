namespace Domain.Model;

public class CompanyToGroupConnection
{
    public int SpolkiId { get; set; }
    public Spolki Spolki { get; set; }
    public int GroupId { get; set; }
    public Group Group { get; set; }
}