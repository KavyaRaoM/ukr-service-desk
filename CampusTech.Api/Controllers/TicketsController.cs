using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


[ApiController] 
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetTickets()
    {
        if (User.IsInRole("Admin"))
        {
            List<Ticket> allTickets =
                await _ticketService.GetAllAsync();

            List<AdminTicketDto> adminTickets =
                allTickets.Select(ticket =>
                    new AdminTicketDto
                    {
                        Id = ticket.Id,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Priority = ticket.Priority,
                        IsResolved = ticket.IsResolved,
                        Technician = ticket.Technician,
                        CreatedAt = ticket.CreatedAt,
                        SubmittedByName = ticket.User?.Name,
                        SubmittedByEmail = ticket.User?.Email
                    }
                ).ToList();

            return Ok(adminTickets);
        }

        string? userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId =
            int.Parse(userIdClaim);

        List<Ticket> userTickets =
            await _ticketService.GetByUserIdAsync(userId);

        return Ok(userTickets);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(int id)
    {
        Ticket? ticket =
            await _ticketService.GetByIdAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        // Admins are allowed to view any ticket.
        if (User.IsInRole("Admin"))
        {
            return Ok(ticket);
        }

        string? userIdClaim =
            User.FindFirstValue(
                ClaimTypes.NameIdentifier
            );

        if (userIdClaim == null)
        {
            return Unauthorized();
        }

        int userId =
            int.Parse(userIdClaim);

        // Normal users may only view their own tickets.
        if (ticket.UserId != userId)
        {
            return Forbid();
        }

        return Ok(ticket);
    }


    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketDto dto)
    {
        string? userIdClaim =
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        int userId = int.Parse(userIdClaim);

        Ticket newTicket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            IsResolved = false,
            Technician = null,
            UserId = userId
        };

        Ticket createdTicket =
            await _ticketService.AddAsync(newTicket);

        return Created(
            $"/api/tickets/{createdTicket.Id}",
            createdTicket
        );
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketAsync(
        int id,
        UpdateTicketDto dto)
    {
        Ticket? existingTicket =
            await _ticketService.GetByIdAsync(id);

        if (existingTicket == null)
        {
            return NotFound();
        }

        // Admins can update any ticket.
        // Normal users may only update their own ticket.
        if (!User.IsInRole("Admin"))
        {
            string? userIdClaim =
                User.FindFirstValue(
                    ClaimTypes.NameIdentifier
                );

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            int userId =
                int.Parse(userIdClaim);

            if (existingTicket.UserId != userId)
            {
                return Forbid();
            }
        }

        Ticket updatedTicket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority
        };

        bool updated =
            await _ticketService.UpdateAsync(
                id,
                updatedTicket
            );

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }
    [HttpPut("{id}/admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AdminUpdateTicket(
        int id,
        AdminUpdateTicketDto dto)
    {
        bool updated =
            await _ticketService.AdminUpdateAsync(id, dto);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")] //Deletion requests via Id are handled here
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        bool deleted = await _ticketService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

}

