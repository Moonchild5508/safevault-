# SafeVault (C#) - Demo Secure Code Submission

This repository contains a simplified ASP.NET Core Web API demonstrating:

- Input validation and defense-in-depth checks.
- SQL injection prevention via parameterized LINQ (EF Core) and avoiding string concatenation.
- Authentication using JWTs and role-based authorization (Admin/User).
- XSS mitigation by HTML-encoding user-supplied content before returning to clients.
- Unit tests showing checks for security behaviors.

**Important:** This is a demo project for an assignment. Do NOT use the demo secret keys or plain-text password storage in production.

## Running locally (requires .NET 7+)
1. `cd SafeVault`
2. `dotnet run`

API endpoints:
- `POST /api/auth/login` - body: `{ "username": "admin", "password": "adminpass" }`
- `GET /api/secrets/{id}` - requires Authorization: Bearer & a valid JWT
- `GET /api/secrets/search?q=term` - Admin-only

## What to submit
- A public GitHub repository with these files (optional: push to your GitHub).
- This zip contains the code and unit tests.

