const API_URL = "http://localhost:5003/api";

export async function login(email, password) {
  const response = await fetch(`${API_URL}/auth/login`, {
    method: "POST",

    headers: {
      "Content-Type": "application/json"
    },

    body: JSON.stringify({
      email,
      password
    })
  });

  if (!response.ok) {
    throw new Error("Invalid email or password.");
  }

  return await response.json();
}

export async function register(name, email, password) {
  const response = await fetch(`${API_URL}/auth/register`, {
    method: "POST",

    headers: {
      "Content-Type": "application/json"
    },

    body: JSON.stringify({
      name,
      email,
      password
    })
  });

  if (!response.ok) {
    const errorData = await response.json();
    throw new Error(
      errorData[0]?.description || "Registration failed."
    );
  }

  return await response.json();
}

export async function getTickets() {
  const token = localStorage.getItem("token");

  const response = await fetch(`${API_URL}/tickets`, {
    method: "GET",

    headers: {
      Authorization: `Bearer ${token}`
    }
  });

  if (!response.ok) {
    throw new Error("Could not load tickets.");
  }

  return await response.json();
}

export async function createTicket(ticket) {
  const token = localStorage.getItem("token");

  const response = await fetch(`${API_URL}/tickets`, {
    method: "POST",

    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`
    },

    body: JSON.stringify(ticket)
  });

  if (!response.ok) {
    throw new Error("Could not create ticket.");
  }

  return await response.json();
}

export async function adminUpdateTicket(id, ticket) {
  const token = localStorage.getItem("token");

  const response = await fetch(
    `${API_URL}/tickets/${id}/admin`,
    {
      method: "PUT",

      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },

      body: JSON.stringify(ticket)
    }
  );

  if (!response.ok) {
    throw new Error("Could not update ticket.");
  }
}

export async function deleteTicket(id) {
  const token = localStorage.getItem("token");

  const response = await fetch(
    `${API_URL}/tickets/${id}`,
    {
      method: "DELETE",

      headers: {
        Authorization: `Bearer ${token}`
      }
    }
  );

  if (!response.ok) {
    throw new Error("Could not delete ticket.");
  }

  
}

export async function registerAdmin(
  name,
  email,
  password
) {
  const token = localStorage.getItem("token");

  const response = await fetch(
    `${API_URL}/auth/register-admin`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify({
        name,
        email,
        password
      })
    }
  );

  if (!response.ok) {
    const errorData = await response.json();

    throw new Error(
      errorData[0]?.description ||
      "Could not create administrator."
    );
  }

  return await response.json();
}