using System.ComponentModel.DataAnnotations;
public class UpdateTicketDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1,5)]
    public int Priority { get; set; }
    
}