namespace Domain.Model;

public class Estimate
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public float EstimateValue { get; set; }
    public Company Company { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}