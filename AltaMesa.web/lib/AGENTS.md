# AGENTS.md

# AltaMesa — Development Agent Rules

## Project Context

This project is developed under the Software Development II syllabus.

System:

AltaMesa — Enterprise Restaurant Order Management System.

Purpose:

Build a maintainable enterprise web application using .NET Framework and MVC architecture while respecting academic software engineering standards and a centralized design system.

Main modules:

* Authentication
* User administration
* Table management
* Product management
* Order lifecycle
* Kitchen workflow
* Real-time communication

---

# Mandatory Technology Stack

## Backend

* C#
* ASP.NET MVC 5
* .NET Framework 4.8

---

## Data Access

* ADO.NET
* Entity Framework 6
* LINQ
* Stored Procedures

---

## Database

* SQL Server 2022

---

## Frontend

* Razor Views (.cshtml)
* Bootstrap 5
* JavaScript
* jQuery

---

## Real Time Communication

* SignalR 2

SignalR is mandatory for:

* Kitchen updates
* Order notifications
* Additional item notifications
* Delivery state updates

---

## Development Environment

* Visual Studio 2022

---

# Documentation Hierarchy

All generation must follow this order.

Priority:

1. DATABASE.md
2. ARCHITECTURE.md
3. DESIGN.md
4. AGENTS.md

Rules:

* DATABASE defines data.
* ARCHITECTURE defines structure.
* DESIGN defines UI.
* AGENTS defines execution rules.

Lower priority documents must never override higher priority documents.

---

# Folder Structure

Project must respect:

lib/
├── DATABASE.md
├── ARCHITECTURE.md
├── DESIGN.md
└── AGENTS.md

Controllers/
Data/
DTOs/
Filters/
Helpers/
Hubs/
Models/
Repositories/
Services/
Views/

---

# Architecture

Mandatory architecture:

MVC + Service Layer + Repository Pattern

Application flow:

Controller
→ Service
→ Repository
→ SQL / Stored Procedure

Real-time flow:

Controller
→ Service
→ NotificationService
→ Hub
→ Client

---

# Controller Responsibilities

Controllers must:

* Receive requests
* Validate ModelState
* Call Services
* Return Views
* Redirect navigation

Controllers must NOT:

* Execute SQL
* Use DbContext
* Execute Stored Procedures
* Contain business logic
* Open transactions
* Generate UI

Controllers remain thin.

---

# Service Responsibilities

Services must:

* Execute business rules
* Coordinate repositories
* Trigger notifications
* Execute validations
* Manage transactions

Services must NOT:

* Generate Views
* Access Session directly
* Build HTML

Example:

PedidoController
→ PedidoService
→ PedidoRepository

---

# Repository Responsibilities

Repositories must:

* Access SQL Server
* Execute SP
* Execute queries

Repositories must NOT:

* Execute business rules
* Generate UI
* Generate ViewModels

Repositories are the only layer allowed to access:

* ADO.NET
* EF6
* Stored Procedures

---

# Model Rules

Entities represent persistence.

Examples:

* Usuario
* Pedido
* Mesa
* Producto

Entities must:

* Use DataAnnotations
* Define validation

Entities must NOT:

* Execute business logic
* Access databases

---

# ViewModel Rules

ViewModels are mandatory.

Preferred:

@model PedidoVM

Avoid:

@model Pedido

ViewModels must:

* Shape UI data
* Support validation

---

# DTO Rules

DTO usage:

Repository
→ Service

Service
→ Controller

DTOs must NOT:

* Access database
* Define presentation rules

---

# Validation Standards

Mandatory:

* DataAnnotations
* ModelState
* Server validation

Example:

if (!ModelState.IsValid)
{
return View(model);
}

Persistence without validation is forbidden.

---

# Data Access Rules

Priority:

1. Stored Procedures
2. ADO.NET
3. Transactions
4. Entity Framework 6
5. LINQ

Use SP for:

* Business operations
* Create
* Update
* Delete

Use EF6 for:

* Read
* Reports
* Dashboard

Avoid:

* SQL in controllers
* Dynamic SQL

---

# Stored Procedure Rules

Business operations must be centralized.

Examples:

* sp_login
* sp_crear_pedido
* sp_agregar_detalle_pedido
* sp_agregar_adicional_pedido
* sp_cerrar_pedido

Repositories execute SP.

Controllers never execute SP.

---

# SignalR Rules

Use:

* PedidoHub
* CocinaHub
* NotificationService

Do not:

* Call hubs directly from Views
* Place business logic inside hubs

---

# UI Rules

UI generation MUST follow DESIGN.md.

Never infer:

* Colors
* Typography
* Component spacing
* Border radius
* Elevation
* Navigation style

Always respect:

* Design tokens
* Component definitions
* Layout system
* Responsive behavior

Bootstrap exists only as implementation.

Bootstrap must NOT override DESIGN.md.

---

# Razor Rules

Use:

Strongly typed views.

Preferred:

@model PedidoVM

Avoid:

* ViewBag
* ViewData
* Inline business logic

---

# Security Rules

Mandatory:

* Parameterized queries
* Input validation
* AntiForgeryToken
* Session validation
* Password hashing
* Exception handling

Never:

* Concatenate SQL
* Store passwords
* Expose secrets

---

# Authentication Rules

Authentication:

Login
→ Service
→ Repository
→ Session

Authorization:

Role-based.

Roles:

* Administrador
* Mesero
* Chef

---

# Business Rules

Restaurant rules:

* One active order per table
* Orders remain open
* Additional products allowed
* Kitchen works by item
* Partial delivery supported
* Closing releases table

States are defined exclusively in DATABASE.md.

---

# Forbidden Technologies

Do NOT generate:

* ASP.NET Core
* EF Core
* Blazor
* Razor Pages
* Minimal APIs
* React
* Angular
* Vue
* Microservices

Unless explicitly requested.

---

# Code Generation Rules

When generating code:

1. Respect DATABASE.md
2. Respect ARCHITECTURE.md
3. Respect DESIGN.md
4. Respect AGENTS.md
5. Keep Controllers thin
6. Use DTOs
7. Use ViewModels
8. Use Services
9. Use Repositories
10. Prefer Stored Procedures
11. Use SignalR
12. Generate maintainable code
