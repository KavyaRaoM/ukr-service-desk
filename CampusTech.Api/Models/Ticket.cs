public class Ticket
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Priority { get; set; }

    public bool IsResolved { get; set; }

    public string? Technician { get; set; }

    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    public int? UserId { get; set; }

    public User? User { get; set; }
}