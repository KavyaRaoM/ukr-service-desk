
using Microsoft.EntityFrameworkCore;
public class TicketService : ITicketService
{
    private readonly CampusTechDbContext _context;


    public TicketService(CampusTechDbContext context)
    {
        _context = context;
    }


//Get all Tickets
    public async Task<List<Ticket>> GetAllAsync()
    {
        return await _context.Tickets
        .Include(t => t.User)
        .ToListAsync();
    }


//Create a new Ticket
    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        _context.Tickets.Add(ticket); 
        await _context.SaveChangesAsync();

        return ticket;
    }

//Get a ticket via ID
    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
                                    .FirstOrDefaultAsync(t => t.Id == id);
    }
     
//Update an existing ticket     
    public async Task<bool> UpdateAsync(int id, Ticket updatedTicket)
    {
        Ticket? existingTicket = await _context.Tickets
                                                    .FirstOrDefaultAsync(t => t.Id == id);

        if (existingTicket == null)
        {
            return false;
        }

        existingTicket.Title = updatedTicket.Title;
        existingTicket.Description = updatedTicket.Description;
        existingTicket.Priority = updatedTicket.Priority;
        

        await _context.SaveChangesAsync();

        return true;
                                        
    }

//Delete an existing ticket    
    public async Task<bool> DeleteAsync(int id)
    {
        Ticket? existingTicket = await _context.Tickets
                                                    .FirstOrDefaultAsync(t => t.Id == id);

        if (existingTicket == null)
        {
            return false;
        }

        _context.Tickets.Remove(existingTicket);
        await _context.SaveChangesAsync();

        return true;

    }


    public async Task<List<Ticket>> GetByUserIdAsync(int userId)
    {
        return await _context.Tickets
            .Where(t => t.UserId == userId)
            .ToListAsync();
    }
     
    public async Task<bool> AdminUpdateAsync(
    int id,
    AdminUpdateTicketDto dto)
    {
        Ticket? ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.Id == id);

        if (ticket == null)
            return false;

        ticket.Priority = dto.Priority;
        ticket.IsResolved = dto.IsResolved;
        ticket.Technician = dto.Technician;

        await _context.SaveChangesAsync();

        return true;
    }
}