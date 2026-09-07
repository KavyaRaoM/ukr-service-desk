public interface ITicketService
{
    Task<List<Ticket>> GetAllAsync();
    Task<List<Ticket>> GetByUserIdAsync(int userId);
    Task<Ticket?> GetByIdAsync(int id);
    Task<Ticket> AddAsync(Ticket ticket);
    Task<bool> UpdateAsync(int id, Ticket updatedTicket);
    Task<bool> DeleteAsync(int id);
    Task<bool> AdminUpdateAsync(int id, AdminUpdateTicketDto dto);


}
