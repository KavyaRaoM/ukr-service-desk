// Console.WriteLine("Hello, World!");

using System.IO.Compression;

Ticket ticket1= new Ticket
{ Id=101,
Title= "Projector Not Displaying",
Description="Projector does not detect the classroom computer.",
Priority=5,
IsResolved= false,
Technician= null
};

Ticket ticket2 = new Ticket 
{ Id = 102, 
Title= "Software installation",
Description= " I need to install COMSOL",
Priority= 2, 
IsResolved=true, 
Technician="Mario" };

Ticket ticket3 = new Ticket 
{ Id = 103, 
Title ="Computer will not boot", 
Description = "Computer is not turning ON", 
Priority = 4, 
IsResolved = false, 
Technician = "Damian" };

List<Ticket> tickets= new List<Ticket>();

tickets.Add(ticket1);
tickets.Add(ticket2);
tickets.Add(ticket3);

foreach(Ticket ticket in tickets)
{

 Console.WriteLine("----------------");
 Console.WriteLine($"Ticket:{ticket.Id}");
 Console.WriteLine($"Priority:{ticket.Priority}");
 Console.WriteLine($"Title:{ticket.Title}");
 if(ticket.IsResolved)
    {
        Console.WriteLine("Status: Resolved");
    }
    else
    {
        Console.WriteLine("Status: Open");
    }

if(ticket.Technician==null)
   {
        Console.WriteLine("Technician: Unassigned");
    }
    else
    {
        Console.WriteLine($"Technician:{ticket.Technician}");
    }
if(ticket.Priority>=4)
   {
        Console.WriteLine("****HIGH PRIORITY****");
    }
}

//Open tickets
List<Ticket> OpenTickets= tickets
                          .Where(t=> !t.IsResolved)
                          .ToList();


List<Ticket> highPriorityTickets = tickets
                          .Where(t=> t.Priority>=4)
                          .ToList();                        

// 3. Get only the titles of resolved tickets

List<string> resolvedTitles = tickets
                          .Where(t=> t.IsResolved)
                          .Select(t=>t.Title)
                          .ToList();  
// 4. Find Ticket ID 103.
// It might not exist.

Ticket? foundTicket = tickets
                .FirstOrDefault(t=> t.Id==103);


Console.WriteLine("Open tickets:");

foreach (Ticket t in OpenTickets)
{
    Console.WriteLine($"{t.Id} - {t.Title}");
}

Console.WriteLine("High priority tickets:");

foreach (Ticket t in highPriorityTickets)
{
    Console.WriteLine($"{t.Id} - {t.Title} - Priority {t.Priority}");
}
Console.WriteLine("Resolved titles:");

foreach (string title in resolvedTitles)
{
    Console.WriteLine(title);
}


if (foundTicket != null)
{
    Console.WriteLine($"Ticket found: {foundTicket.Id} - {foundTicket.Title}");
}
else
{
    Console.WriteLine("Ticket not found.");
}

Console.WriteLine("Starting ticket lookup...");

string titlenew = await GetTicketTitleAsync();

Console.WriteLine($"Found: {titlenew}");

static async Task<string> GetTicketTitleAsync()
{
    Console.WriteLine("Searching...");

    await Task.Delay(2000);

    return "Projector";
}