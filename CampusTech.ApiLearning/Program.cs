using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args); //setup the configuration

builder.Services.AddControllers(); //ASP.NET, add the services/features necessary for me to use controllers.

builder.Services.AddScoped<ITicketService, TicketService>(); //DI, creates one service instance for app lifetime
//Whenever someone asks for an ITicketService, give them a TicketService object.

builder.Services.AddDbContext<CampusTechDbContext>(options => //register our EF Core database context with DI
    options.UseNpgsql( //this DbContext should use PostgreSQL
        builder.Configuration.GetConnectionString("CampusTechDatabase") //retrieve the connection string we stored in configuration/user secrets
    )
);

var app = builder.Build(); //build on the setup that is configured already
app.MapControllers(); //Find the routes defined by my controllers and make them available as endpoints.
app.Run(); //start the application

// List<Ticket> tickets= new List<Ticket>();

//     Ticket ticket1 = new Ticket
//     {
//         Id = 101,
//         Title = "Projector not displaying",
//         Description = "Projector does not detect the classroom computer.",
//         Priority = 5,
//         IsResolved = false,
//         Technician = null
//     };

//     Ticket ticket2 = new Ticket
//     {
//         Id = 102,
//         Title = "COMSOL installation",
//         Description = "COMSOL needs to be installed.",
//         Priority = 2,
//         IsResolved = true,
//         Technician = "Mario"
//     };

//     Ticket ticket3= new Ticket
//     {
//         Id=103,
//         Title="Computer will not boot",
//         Description="Computer does not turn on",
//         Priority=4,
//         IsResolved=false,
//         Technician="Damian"
//     };

//     tickets.Add(ticket1);
//     tickets.Add(ticket2);
//     tickets.Add(ticket3);


// app.MapGet("/api/tickets", () =>
// {
    
//     return tickets
//            .Select(t=> //here select is transforming each full ticket to ticket 
//                        //with just ID and Title
//            new    //creats an anonymous object
//            {
//               t.Id,
//               t.Title
//            })
//            .ToList();
    
    
// });

// app.MapGet("/api/tickets/{id}",(int id)=>
// {
    
//     Ticket? foundTicket= tickets
//                     .FirstOrDefault(t=>t.Id==id);
//     if (foundTicket==null)
//     {
//         return Results.NotFound();
//     }

//     return Results.Ok(foundTicket);
                  

// });

// app.MapPost("/api/tickets", (Ticket newTicket) =>
// {
//     tickets.Add(newTicket);
    
//     return Results.Created(
//         $"/api/tickets/{newTicket.Id}", // where the new resource lives
//         newTicket);                     // response body
// });

// app.MapPut("/api/tickets/{id}", (int id, Ticket updatedTicket) =>
// {
//     Ticket? existingTicket = tickets
//         .FirstOrDefault(t => t.Id == id);

//     if (existingTicket == null)
//     {
//         return Results.NotFound();
//     }

//     existingTicket.Title = updatedTicket.Title;
//     existingTicket.Description = updatedTicket.Description;
//     existingTicket.Priority = updatedTicket.Priority;
//     existingTicket.IsResolved = updatedTicket.IsResolved;
//     existingTicket.Technician = updatedTicket.Technician;

//     return Results.Ok(existingTicket);
// });

// app.MapDelete("/api/tickets/{id}", (int id) =>
// {
//     Ticket? existingTicket = tickets
//         .FirstOrDefault(t => t.Id == id);

//     if (existingTicket == null)
//     {
//         return Results.NotFound();
//     }

//     tickets.Remove(existingTicket);

//     return Results.NoContent();
// });




//curl -i -X POST http://localhost:5072/api/tickets \
//   -H "Content-Type: application/json" \
//   -d '{
//     "id": 104,
//     "title": "Printer not working",
//     "description": "Printer is showing an offline error.",
//     "priority": 3,
//     "isResolved": false,
//     "technician": null
//   }'