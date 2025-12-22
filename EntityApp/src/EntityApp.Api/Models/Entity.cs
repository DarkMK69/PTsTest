namespace EntityApp.Api.Models;

public class Entity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; }
    public PriorityLevel Priority { get; set; }
    public List<string> Tags { get; set; } = [];
    public Dictionary<string, string> Metadata { get; set; } = [];
    public double Rating { get; set; }
    public long Counter { get; set; }
    public string? Website { get; set; }
    public DateOnly? BirthDate { get; set; }
    public TimeOnly? AlarmTime { get; set; }
    public Guid? RefId { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public byte[]? Avatar { get; set; }
}
