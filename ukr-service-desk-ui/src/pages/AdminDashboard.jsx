import {
  useEffect,
  useState
} from "react";

import { useNavigate } from "react-router";

import {
  adminUpdateTicket,
  deleteTicket,
  getTickets,
  registerAdmin
} from "../api";

function AdminDashboard() {
  const navigate = useNavigate();

  const user = JSON.parse(
    localStorage.getItem("user")
  );

  const [tickets, setTickets] = useState([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);

  const [showAdminForm, setShowAdminForm] =
    useState(false);

  const [adminName, setAdminName] =
    useState("");

  const [adminEmail, setAdminEmail] =
    useState("");

  const [adminPassword, setAdminPassword] =
    useState("");

  const [adminMessage, setAdminMessage] =
    useState("");

  const [creatingAdmin, setCreatingAdmin] =
    useState(false);

  useEffect(() => {
    loadTickets();
  }, []);

  async function loadTickets() {
    try {
      setLoading(true);
      setError("");

      const data = await getTickets();

      setTickets(data);

    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }

  async function handleCreateAdmin(event) {
  event.preventDefault();

  setError("");
  setAdminMessage("");
  setCreatingAdmin(true);

  try {
    await registerAdmin(
      adminName,
      adminEmail,
      adminPassword
    );

    setAdminMessage(
      "Administrator created successfully."
    );

    setAdminName("");
    setAdminEmail("");
    setAdminPassword("");

    setShowAdminForm(false);

  } catch (error) {
    setError(error.message);
  } finally {
    setCreatingAdmin(false);
  }
}

  function changeTicketField(
    id,
    field,
    value
  ) {
    setTickets((currentTickets) =>
      currentTickets.map((ticket) =>
        ticket.id === id
          ? {
              ...ticket,
              [field]: value
            }
          : ticket
      )
    );
  }

  async function handleSave(ticket) {
    try {
      setError("");

      await adminUpdateTicket(
        ticket.id,
        {
          priority: Number(ticket.priority),

          isResolved:
            Boolean(ticket.isResolved),

          technician:
            ticket.technician || null
        }
      );

      await loadTickets();

    } catch (error) {
      setError(error.message);
    }
  }

  async function handleDelete(id) {
    const confirmed = window.confirm(
      `Delete ticket #${id}?`
    );

    if (!confirmed) {
      return;
    }

    try {
      setError("");

      await deleteTicket(id);

      await loadTickets();

    } catch (error) {
      setError(error.message);
    }
  }

  function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    navigate("/");
  }

  const openTickets =
    tickets.filter(
      (ticket) => !ticket.isResolved
    ).length;

  const resolvedTickets =
    tickets.filter(
      (ticket) => ticket.isResolved
    ).length;

  return (
    <div>

      <header className="topbar admin-topbar">

        <div>
          <h1>UKR Service Desk</h1>
          <p>Administrator Dashboard</p>
        </div>

        <div className="user-area">
          <span>
            {user?.name} · Administrator
          </span>

          <button
            className="secondary-button"
            onClick={() =>
              setShowAdminForm(true)
            }
          >
            Register an Admin
          </button>

          <button
            className="secondary-button"
            onClick={logout}
          >
            Logout
          </button>
        </div>

      </header>

      <main className="dashboard">

              
        {showAdminForm && (
        <section className="panel">
          <div className="panel-heading">
            <div>
              <h2>Register Administrator</h2>

              <p>
                Create another authorized
                service desk administrator.
              </p>
            </div>
          </div>

          {adminMessage && (
            <div className="success-message">
              {adminMessage}
            </div>
          )}

          <form onSubmit={handleCreateAdmin}>
            <label>Name</label>

            <input
              type="text"
              placeholder="Example: Kavya Rao"
              value={adminName}
              onChange={(event) =>
                setAdminName(event.target.value)
              }
              required
            />

            <label>Email</label>

            <input
              type="email"
              placeholder="a-kavya@ukr.edu"
              value={adminEmail}
              onChange={(event) =>
                setAdminEmail(event.target.value)
              }
              required
            />

            <label>Password</label>

            <input
              type="password"
              placeholder="Minimum 6 characters"
              value={adminPassword}
              onChange={(event) =>
                setAdminPassword(event.target.value)
              }
              minLength="6"
              required
            />

            <div className="admin-actions">
              <button
                className="primary-button"
                type="submit"
                disabled={creatingAdmin}
              >
                {creatingAdmin
                  ? "Creating..."
                  : "Create Administrator"}
              </button>

              <button
                className="secondary-button"
                type="button"
                onClick={() =>
                  setShowAdminForm(false)
                }
              >
                Cancel
              </button>
            </div>
          </form>
        </section>
      )}

        <section className="dashboard-heading">

          <div>
            <h2>Service Desk Overview</h2>

            <p>
              Manage support tickets,
              technicians and resolutions.
            </p>
          </div>

        </section>

        <section className="stats-grid">

          <div className="stat-card">
            <strong>
              {tickets.length}
            </strong>

            <span>
              Total Tickets
            </span>
          </div>

          <div className="stat-card">
            <strong>
              {openTickets}
            </strong>

            <span>
              Open
            </span>
          </div>

          <div className="stat-card">
            <strong>
              {resolvedTickets}
            </strong>

            <span>
              Resolved
            </span>
          </div>

        </section>

        {error && (
          <div className="error-message">
            {error}
          </div>
        )}

        <section className="panel">

          <div className="panel-heading">

            <div>
              <h2>All Tickets</h2>

              <p>
                Update ticket priority,
                technician and status.
              </p>
            </div>

            <button
              className="secondary-button"
              onClick={loadTickets}
            >
              Refresh
            </button>

          </div>

          {loading ? (

            <p>Loading tickets...</p>

          ) : tickets.length === 0 ? (

            <div className="empty-state">
              No tickets available.
            </div>

          ) : (

            <div className="admin-ticket-list">

              {tickets.map((ticket) => (

                <article
                  className="admin-ticket-card"
                  key={ticket.id}
                >

                  <div className="ticket-header">

                    <div>
                      <span className="ticket-id">
                        Ticket #{ticket.id}
                      </span>

                      <h3>
                        {ticket.title}
                      </h3>
                    </div>

                    <span
                      className={
                        ticket.isResolved
                          ? "status resolved"
                          : "status open"
                      }
                    >
                      {ticket.isResolved
                        ? "Resolved"
                        : "Open"}
                    </span>

                  </div>

                  <p>
                    {ticket.description}
                  </p>

                  <div className="ticket-meta">
                      <span>
                        Submitted by:{" "}
                        {ticket.submittedByName || "Unknown user"}
                      </span>

                      <span>
                        Email:{" "}
                        {ticket.submittedByEmail || "Not available"}
                      </span>

                      <span>
                        Created:{" "}
                        {ticket.createdAt &&
                        new Date(ticket.createdAt).getFullYear() > 2000
                          ? new Date(ticket.createdAt).toLocaleString(
                              "en-US",
                              {
                                timeZone: "America/Chicago",
                                month: "short",
                                day: "numeric",
                                year: "numeric",
                                hour: "numeric",
                                minute: "2-digit",
                                timeZoneName: "short"
                              }
                            )
                          : "Created before timestamp tracking"}
                    </span>
                </div>

                  <div className="admin-fields">

                    <div>

                      <label>
                        Priority
                      </label>

                      <select
                        value={ticket.priority}
                        onChange={(event) =>
                          changeTicketField(
                            ticket.id,
                            "priority",
                            Number(
                              event.target.value
                            )
                          )
                        }
                      >
                        <option value="1">
                          1 - Low
                        </option>

                        <option value="2">
                          2
                        </option>

                        <option value="3">
                          3 - Medium
                        </option>

                        <option value="4">
                          4
                        </option>

                        <option value="5">
                          5 - Critical
                        </option>
                      </select>

                    </div>

                    <div>

                      <label>
                        Technician
                      </label>

                      <input
                        type="text"
                        placeholder="Technician name"
                        value={
                          ticket.technician || ""
                        }
                        onChange={(event) =>
                          changeTicketField(
                            ticket.id,
                            "technician",
                            event.target.value
                          )
                        }
                      />

                    </div>

                    <div>

                      <label>
                        Status
                      </label>

                      <select
                        value={
                          ticket.isResolved
                            ? "resolved"
                            : "open"
                        }
                        onChange={(event) =>
                          changeTicketField(
                            ticket.id,
                            "isResolved",
                            event.target.value ===
                              "resolved"
                          )
                        }
                      >
                        <option value="open">
                          Open
                        </option>

                        <option value="resolved">
                          Resolved
                        </option>
                      </select>

                    </div>

                  </div>

                  <div className="admin-actions">

                    <button
                      className="primary-button small-button"
                      onClick={() =>
                        handleSave(ticket)
                      }
                    >
                      Save Changes
                    </button>

                    <button
                      className="danger-button"
                      onClick={() =>
                        handleDelete(ticket.id)
                      }
                    >
                      Delete
                    </button>

                  </div>

                </article>

              ))}

            </div>

          )}

        </section>

      </main>

    </div>
  );
}

export default AdminDashboard;