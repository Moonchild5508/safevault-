SafeVault (C#) - Demo Secure Code Submission
This repository contains a simplified ASP.NET Core Web API demonstrating:

Input validation and defense-in-depth checks.
SQL injection prevention via parameterized LINQ (EF Core) and avoiding string concatenation.
Authentication using JWTs and role-based authorization (Admin/User).
XSS mitigation by HTML-encoding user-supplied content before returning to clients.
Unit tests showing checks for security behaviors.
