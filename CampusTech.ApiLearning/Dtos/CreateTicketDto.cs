using System.ComponentModel.DataAnnotations;
//[ApiController] gives us automatic request-model validation behavior.
//So if validation fails, we generally don't need to manually write:
// POST /api/tickets
//         ↓
// JSON arrives
//         ↓
// ASP.NET creates CreateTicketDto
//         ↓
// checks validation attributes
//         ↓
//      valid?
//     /      \
//   yes       no
//    ↓         ↓
// Controller   400 Bad Request
// method
// runs
public class CreateTicketDto
{
    [Required] //attributes like [apicontroller]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1,5)]
    public int Priority { get; set; }
}