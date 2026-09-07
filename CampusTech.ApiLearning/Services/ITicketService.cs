public interface ITicketService
{
    Task<List<Ticket>> GetAllAsync();
    Task<Ticket> AddAsync(Ticket ticket);
    Task<Ticket?> GetByIdAsync(int id);

    Task<bool> UpdateAsync(int id, Ticket updatedTicket);

    Task<bool> DeleteAsync(int id);

    // Ticket? GetById(int id);

    // Ticket Add(Ticket ticket);

    // bool Update(int id, Ticket updatedTicket); //Why bool? Because the service needs to tell the controller whether the update succeeded.
    // bool Delete(int id);

}
