// using Microsoft.EntityFrameworkCore;

// public class CampusTechDbContext : DbContext
// {
//     public CampusTechDbContext(
//         DbContextOptions<CampusTechDbContext> options) : base(options)
//     {
//     }

//     public DbSet<Ticket> Tickets { get; set; }
//     public DbSet<User> Users { get; set; }

// }

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class CampusTechDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public CampusTechDbContext(
        DbContextOptions<CampusTechDbContext> options)
        : base(options)
    {
    }

    public DbSet<Ticket> Tickets { get; set; }
}