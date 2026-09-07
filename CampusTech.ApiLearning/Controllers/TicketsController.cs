using Microsoft.AspNetCore.Mvc;

[ApiController] //Is called an attribute.An attribute adds metadata/instructions about a class or method.
                //This class is an API controller; ASP.NET should apply API-controller behavior to it.
                //other attributes [ApiController],[Route(...)],[HttpGet],[HttpPost],[HttpPut],[HttpDelete],[Route("api/[controller]")]
[Route("api/[controller]")]// Route replaces controller with classname minus controller so here tickets
public class TicketsController : ControllerBase //ASP.NET provides ControllerBase; we're creating our own controller based on it.
{
    private readonly ITicketService _ticketService;

    public TicketsController(ITicketService ticketService)
    {
        _ticketService = ticketService;
    }
    
    [HttpGet] //this method handles GET requests
    public async Task<IActionResult> GetTickets()
    {
        List<Ticket> tickets = await _ticketService.GetAllAsync();

        return Ok(tickets);
    }
    // public IActionResult GetTickets() //regular C# method
    //                                     //IActionResult: this method returns an HTTP-style result
    // {
    //     List<Ticket> tickets = _ticketService.GetAll();
    //     return Ok(tickets); //Ok(...)->200 OK response
    // }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetTicketById(int id)
    {
        Ticket? ticket =
            await _ticketService.GetByIdAsync(id);

        if (ticket == null)
        {
            return NotFound();
        }

        return Ok(ticket);
    }

    // [HttpGet("{id}")]//adds /{id} to url
    // public IActionResult GetTicketById(int id)
    // {
        
    //     Console.WriteLine($"Requested id: {id}");
    //     Ticket? ticket = _ticketService.GetById(id);

    //     if (ticket == null)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(ticket);
    // }

    [HttpPost]
    public async Task<IActionResult> CreateTicket(CreateTicketDto dto)
    {
        Ticket newTicket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
            IsResolved = false,
            Technician = null
        };

        Ticket createdTicket =
            await _ticketService.AddAsync(newTicket);

        return Created(
            $"/api/tickets/{createdTicket.Id}",
            createdTicket
        );
    }

    // [HttpPost]
    // public IActionResult CreateTicket(CreateTicketDto dto)
    // {
        
    //     Ticket newTicket= new Ticket
    //     {

    //         Title= dto.Title,
    //         Description= dto.Title,
    //         Priority=dto.Priority,
    //         IsResolved=false,
    //         Technician=null

    //     };
        
    //     Ticket createdTicket=_ticketService.Add(newTicket);

    //     return Created(
    //         $"/api/tickets/{createdTicket.Id}",
    //         createdTicket
    //     );
    // }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTicketAsync(int id, UpdateTicketDto dto)
    {
        Ticket updatedTicket = new Ticket
        {
            Title = dto.Title,
            Description = dto.Description,
            Priority = dto.Priority,
    
        };

        bool updated =
            await _ticketService.UpdateAsync(id, updatedTicket);

        if (!updated)
        {
            return NotFound();
        }

        return Ok(updatedTicket);
    }

    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateTicketAsync(int id, Ticket updatedTicket)
    // {
    //     bool updated = await _ticketService.UpdateAsync(id, updatedTicket);
    //     if (!updated)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(updatedTicket);
    // }


    // [HttpPut("{id}")]
    // public IActionResult UpdateTicket(int id, Ticket updatedTicket)
    // {
    //     bool updated = _ticketService.Update(id, updatedTicket);

    //     if (!updated)
    //     {
    //         return NotFound();
    //     }

    //     return Ok(updatedTicket);
    // }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTicket(int id)
    {
        bool deleted = await _ticketService.DeleteAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }


    // [HttpDelete("{id}")]
    // public IActionResult DeleteTicket(int id)
    // {
    //     bool deleted = _ticketService.Delete(id);

    //     if (!deleted)
    //     {
    //         return NotFound();
    //     }

    //     return NoContent();
    // }
}


// Your controller says:

// public class TicketsController : ControllerBase

// ControllerBase provides convenient methods such as:

// Ok(...)          // 200
// Created(...)     // 201
// NoContent()      // 204
// BadRequest(...)  // 400
// NotFound()       // 404

// Those methods produce objects that implement IActionResult.

// So this:

// return Ok(ticket);

// doesn't mean "return the Ticket directly."

// It means:

// Build an HTTP 200 OK response and put this ticket in its response body.