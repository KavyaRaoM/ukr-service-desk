using Microsoft.EntityFrameworkCore;

public class CampusTechDbContext : DbContext
{
    public CampusTechDbContext(
        DbContextOptions<CampusTechDbContext> options) //contains configuration for this context, 
                                                    //such as which database provider and connection string to use.
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; } //This context manages Ticket entities.
}