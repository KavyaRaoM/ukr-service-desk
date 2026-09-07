
using Microsoft.EntityFrameworkCore;
public class TicketService : ITicketService
{
    private readonly CampusTechDbContext _context;


    public TicketService(CampusTechDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetAllAsync()
    {
        return await _context.Tickets.ToListAsync();
    }

    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        _context.Tickets.Add(ticket); //EF Core starts tracking new Ticket

        await _context.SaveChangesAsync(); //EF Core generates INSERT
                                        //Add() alone doesn't actually save the row to PostgreSQL. This line is critical:

        return ticket;
    }

    public async Task<Ticket?> GetByIdAsync(int id)
    {
        return await _context.Tickets
                                    .FirstOrDefaultAsync(t => t.Id == id);
    }
        

    // public Ticket? GetById(int id)
    // {
    //         // return _tickets.FirstOrDefault(t => t.Id == id);
    //         Console.WriteLine($"Service searching for: {id}");

    //     Ticket? ticket = _tickets.FirstOrDefault(t => t.Id == id);

    //     Console.WriteLine(ticket == null
    //         ? "Service found: NULL"
    //         : $"Service found: {ticket.Id} - {ticket.Title}");

    //     return ticket;
    // }
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

    // public Ticket Add(Ticket ticket)
    // {
    //     int nextId = _tickets.Max(t => t.Id) + 1;

    //     ticket.Id = nextId;
    //     _tickets.Add(ticket);
    //     return ticket;
    // }


    // public bool Update(int id, Ticket updatedTicket)
    // {
    //     Ticket? existingTicket = _tickets
    //         .FirstOrDefault(t => t.Id == id);

    //     if (existingTicket == null)
    //     {
    //         return false;
    //     }

    //     existingTicket.Title = updatedTicket.Title;
    //     existingTicket.Description = updatedTicket.Description;
    //     existingTicket.Priority = updatedTicket.Priority;
    //     existingTicket.IsResolved = updatedTicket.IsResolved;
    //     existingTicket.Technician = updatedTicket.Technician;

    //     return true;
    // }

    // public bool Delete(int id)
    // {
    //     Ticket? existingTicket = _tickets
    //     .FirstOrDefault(t => t.Id == id);

    //     if (existingTicket == null)
    //     {
    //         return false;
    //     }

    //     _tickets.Remove(existingTicket);

    //     return true;
     
}