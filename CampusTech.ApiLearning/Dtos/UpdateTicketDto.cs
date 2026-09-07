using System.ComponentModel.DataAnnotations;
public class UpdateTicketDto
{
    [Required] //attributes like [apicontroller]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1,5)]
    public int Priority { get; set; }
    // public bool IsResolved { get; set; }

    // public string? Technician { get; set; }
}