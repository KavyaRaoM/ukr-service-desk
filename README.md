# UKR Service Desk

A full-stack university IT service desk application for submitting, tracking, and managing technical support tickets.

The application provides separate workflows for users and administrators, including secure authentication, role-based authorization, ticket ownership, technician assignment, priority management, and ticket resolution.

## Application Preview

### Login

![UKR Service Desk Login](screenshots/login-page.png)

### User Dashboard

Users can submit support requests and track the status of their tickets.

![UKR Service Desk User Dashboard](screenshots/User-dashboard.png)

### Administrator Dashboard

Administrators can monitor all submitted tickets, assign technicians, update priorities and statuses, and manage service desk activity.

![UKR Service Desk Administrator Dashboard](screenshots/Admin-dashboard.png)

## Features

### User Portal
- Register and log in securely
- Create IT support tickets
- Set ticket priority
- View personal support tickets
- Track ticket status and technician assignment
- Users can access only their own tickets

### Administrator Portal
- Separate administrator dashboard
- View tickets submitted by all users
- View submitter name, email, and ticket creation time
- Assign technicians
- Change ticket priority
- Mark tickets as resolved or reopen them
- Delete tickets
- Register additional administrators

## Technology Stack

**Frontend**
- React
- JavaScript
- Vite
- React Router
- HTML5 / CSS3

**Backend**
- C#
- ASP.NET Core Web API
- Entity Framework Core
- ASP.NET Core Identity

**Database**
- PostgreSQL

**Authentication & Security**
- JSON Web Tokens (JWT)
- ASP.NET Core Identity
- Role-based authorization
- Password hashing through ASP.NET Core Identity
- User-based ticket ownership
- Protected API endpoints

## Architecture

The application follows a layered full-stack architecture:

```text
React Frontend
      │
      │ HTTP / JSON
      ▼
ASP.NET Core Web API
      │
      ▼
Controllers
      │
      ▼
Service Layer
      │
      ▼
Entity Framework Core
      │
      ▼
PostgreSQL
```

The React frontend communicates with the ASP.NET Core REST API using HTTP requests. Controllers handle incoming requests and delegate ticket operations to the service layer. Entity Framework Core handles database access and persistence in PostgreSQL.

## Authentication Flow

```text
User Login
    │
    ▼
ASP.NET Core Identity
verifies credentials
    │
    ▼
JWT generated
    │
    ▼
React stores authentication token
    │
    ▼
Token sent with protected API requests
    │
    ▼
ASP.NET Core validates token and role
    │
    ▼
Authorized resource returned
```

The backend determines ticket ownership using the authenticated user's ID from the JWT rather than trusting a user ID supplied by the frontend.

Administrator endpoints are protected using role-based authorization.

## Ticket Workflow

```text
User creates ticket
        │
        ▼
Ticket stored in PostgreSQL
        │
        ▼
Administrator reviews ticket
        │
        ├── Assign technician
        ├── Update priority
        └── Update status
                │
                ▼
          Ticket resolved
```

## API Examples

| Method | Endpoint | Purpose |
|---|---|---|
| POST | `/api/auth/register` | Register a user |
| POST | `/api/auth/login` | Authenticate and receive JWT |
| POST | `/api/auth/register-admin` | Register an administrator |
| GET | `/api/tickets` | Retrieve authorized tickets |
| GET | `/api/tickets/{id}` | Retrieve a ticket |
| POST | `/api/tickets` | Create a ticket |
| PUT | `/api/tickets/{id}` | Update a ticket |
| PUT | `/api/tickets/{id}/admin` | Administrator ticket management |
| DELETE | `/api/tickets/{id}` | Delete a ticket (Admin) |

## Security Design

The application includes several backend security controls:

- Passwords are managed and hashed through ASP.NET Core Identity.
- JWT Bearer authentication protects API resources.
- Role claims distinguish users from administrators.
- Administrative endpoints require the `Admin` role.
- Ticket ownership is derived from the authenticated user's JWT claims.
- Users cannot retrieve or modify another user's tickets.
- Secrets such as database credentials and JWT signing keys are kept outside source control.

Frontend route protection improves the user experience, while authorization is enforced independently by the backend API.

## Project Structure

```text
ukr-service-desk/
│
├── CampusTech.Api/
│   ├── Controllers/
│   ├── Data/
│   ├── Dtos/
│   ├── Models/
│   ├── Services/
│   ├── Migrations/
│   └── Program.cs
│
├── ukr-service-desk-ui/
│   └── src/
│       ├── pages/
│       ├── api.js
│       ├── App.jsx
│       ├── main.jsx
│       └── styles.css
│
└── README.md
```

## Running the Project Locally

### Prerequisites

- .NET SDK
- PostgreSQL
- Node.js and npm

### Backend

Configure the PostgreSQL connection string and JWT signing key using .NET User Secrets.

Then:

```bash
cd CampusTech.Api
dotnet restore
dotnet ef database update
dotnet run
```

The development API is configured to run locally.

### Frontend

In another terminal:

```bash
cd ukr-service-desk-ui
npm install
npm run dev
```

Open the local URL displayed by Vite in your browser.

> Database credentials, JWT signing keys, and demo account passwords are intentionally not included in this repository.

## What I Built

This project was developed as an end-to-end full-stack application, including:

- Relational data modeling with Entity Framework Core
- PostgreSQL database integration and migrations
- REST API design with ASP.NET Core
- Service-layer architecture and dependency injection
- ASP.NET Core Identity integration
- JWT authentication
- User and administrator role authorization
- Resource-level ticket ownership authorization
- React component and state management
- API integration between React and ASP.NET Core
- Responsive user and administrator interfaces
- Error handling and protected application routes

## Future Enhancements

Potential future improvements include:

- Ticket search and filtering
- Pagination
- Email notifications
- File attachments
- Automated testing
- Docker containerization
- Cloud deployment
- Audit logging

## Author

**Kavya Rao**

M.S. Computer Science  
Full-Stack Software Development