using System.ComponentModel.DataAnnotations;

public class AdminUpdateTicketDto
{
    [Range(1, 5)]
    public int Priority { get; set; }

    public bool IsResolved { get; set; }

    public string? Technician { get; set; }
}