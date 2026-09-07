import {
  useEffect,
  useState
} from "react";

import { useNavigate } from "react-router";

import {
  createTicket,
  getTickets
} from "../api";

function UserDashboard() {
  const navigate = useNavigate();

  const user = JSON.parse(
    localStorage.getItem("user")
  );

  const [tickets, setTickets] = useState([]);

  const [title, setTitle] = useState("");
  const [description, setDescription] =
    useState("");

  const [priority, setPriority] = useState(3);

  const [error, setError] = useState("");
  const [loading, setLoading] = useState(true);
  const [creating, setCreating] = useState(false);

  useEffect(() => {
    loadTickets();
  }, []);

  async function loadTickets() {
    try {
      setLoading(true);

      const data = await getTickets();

      setTickets(data);

    } catch (error) {
      setError(error.message);
    } finally {
      setLoading(false);
    }
  }

  async function handleCreateTicket(event) {
    event.preventDefault();

    setError("");
    setCreating(true);

    try {
      await createTicket({
        title,
        description,
        priority: Number(priority)
      });

      setTitle("");
      setDescription("");
      setPriority(3);

      await loadTickets();

    } catch (error) {
      setError(error.message);
    } finally {
      setCreating(false);
    }
  }

  function logout() {
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    navigate("/");
  }

  return (
    <div>

      <header className="topbar">

        <div>
          <h1>UKR Service Desk</h1>
          <p>User Dashboard</p>
        </div>

        <div className="user-area">
          <span>
            Welcome, {user?.name}
          </span>

          <button
            className="secondary-button"
            onClick={logout}
          >
            Logout
          </button>
        </div>

      </header>

      <main className="dashboard">

        <section className="dashboard-heading">
          <div>
            <h2>My Support Tickets</h2>

            <p>
              Create and track your university
              technology support requests.
            </p>
          </div>

          <div className="stat-card">
            <strong>{tickets.length}</strong>
            <span>Total Tickets</span>
          </div>
        </section>

        {error && (
          <div className="error-message">
            {error}
          </div>
        )}

        <div className="dashboard-grid">

          <section className="panel">

            <h2>Create Ticket</h2>

            <form onSubmit={handleCreateTicket}>

              <label>Title</label>

              <input
                type="text"
                value={title}
                placeholder="Example: Projector not working"
                onChange={(event) =>
                  setTitle(event.target.value)
                }
                required
              />

              <label>Description</label>

              <textarea
                value={description}
                placeholder="Describe the issue..."
                onChange={(event) =>
                  setDescription(event.target.value)
                }
                required
              />

              <label>Priority</label>

              <select
                value={priority}
                onChange={(event) =>
                  setPriority(event.target.value)
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

              <button
                className="primary-button"
                type="submit"
                disabled={creating}
              >
                {creating
                  ? "Creating..."
                  : "Submit Ticket"}
              </button>

            </form>

          </section>

          <section className="panel">

            <div className="panel-heading">

              <h2>My Tickets</h2>

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
                <h3>No tickets yet</h3>

                <p>
                  Create your first support ticket.
                </p>
              </div>

            ) : (

              <div className="ticket-list">

                {tickets.map((ticket) => (

                  <article
                    className="ticket-card"
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
                        Priority: {ticket.priority}
                      </span>

                      <span>
                        Technician:{" "}
                        {ticket.technician ||
                          "Not assigned"}
                      </span>

                    </div>

                  </article>

                ))}

              </div>

            )}

          </section>

        </div>

      </main>

    </div>
  );
}

export default UserDashboard;