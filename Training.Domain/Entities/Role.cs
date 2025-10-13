namespace Training.Domain.Entities;

public class Role
{
    public int Id { get; set; }
    public string? RoleName { get; set; }

    // Navigation property
    public ICollection<User> Users { get; set; } = new List<User>();
}