public class AdminTicketDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int Priority { get; set; }

    public bool IsResolved { get; set; }

    public string? Technician { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? SubmittedByName { get; set; }

    public string? SubmittedByEmail { get; set; }
}