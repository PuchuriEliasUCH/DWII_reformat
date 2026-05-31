# ARCHITECTURE.md

# AltaMesa — Architecture Specification

# Architecture Overview

Architecture Style:

ASP.NET MVC 5

Application Type:

Enterprise Restaurant Management System

Framework:

.NET Framework 4.8

Pattern:

MVC + Service Layer + Repository Pattern

Communication:

Request / Response + Real-Time Events

---

# Architectural Principles

Mandatory:

* Separation of Concerns
* Maintainability
* Reusability
* Scalability
* Low Coupling
* High Cohesion
* Single Responsibility
* Explicit Dependencies
* Predictable UI Behavior

---

# Documentation Hierarchy

Architecture must follow:

1. DATABASE.md
2. ARCHITECTURE.md
3. DESIGN.md
4. AGENTS.md

Rules:

DATABASE
defines persistence.

ARCHITECTURE
defines execution structure.

DESIGN
defines UI.

AGENTS
defines generation rules.

Lower levels cannot override higher levels.

---

# Solution Structure

Solution

└── AltaMesa.Web

    ├── App_Start

    ├── Constants

    ├── Content

    ├── Controllers

    ├── Data

    ├── DTOs

    ├── Filters

    ├── Helpers

    ├── Hubs

    ├── Models

    │

    ├── Entities

    └── ViewModels

    ├── Repositories

    ├── Services

    ├── Scripts

    ├── Views

    └── lib

---

# Layer Architecture

## Presentation Layer

Namespace:

AltaMesa.Web

Contains:

* Controllers
* Views
* ViewModels
* Scripts
* Filters

Responsibilities:

* Render UI
* Receive requests
* Validate ModelState
* Return Views

Must NOT:

* Execute SQL
* Execute business rules
* Execute transactions

---

## Application Layer

Namespace:

Services

Contains:

* Services
* Interfaces
* NotificationService

Responsibilities:

* Execute business rules
* Coordinate workflows
* Trigger notifications
* Validate operations

Must NOT:

* Generate HTML
* Render UI

---

## Persistence Layer

Namespace:

Repositories

Contains:

* Repositories
* DbContext
* ADO.NET
* Stored Procedures

Responsibilities:

* Read
* Persist
* Execute SP
* Execute transactions

Must NOT:

* Apply business rules

---

## Domain Layer

Namespace:

Models

Contains:

Entities

DTOs

ViewModels

Responsibilities:

* Represent information
* Define validation

Must NOT:

* Execute infrastructure

---

# Request Flow

Standard Request

Browser

↓

Controller

↓

Service

↓

Repository

↓

ADO.NET / EF6

↓

SQL Server

↓

Repository

↓

Service

↓

Controller

↓

View

---

# Real-Time Flow

Business Event

↓

Service

↓

NotificationService

↓

SignalR Hub

↓

Connected Clients

Examples:

Kitchen updated

↓

Mesero receives update

Additional products

↓

Kitchen receives update

Product ready

↓

Mesero receives update

---

# Dependency Rules

Allowed

Controller
→ Service

Service
→ Repository

Repository
→ SQL

NotificationService
→ Hub

View
→ ViewModel

Forbidden

Controller
→ Repository

Controller
→ SQL

View
→ Service

View
→ Repository

Hub
→ Repository

Repository
→ Controller

---

# Validation Flow

View

↓

ViewModel

↓

DataAnnotations

↓

ModelState

↓

Service Validation

↓

Database Validation

---

# Authentication Architecture

Login

↓

AuthController

↓

AuthService

↓

UsuarioRepository

↓

sp_login

↓

Session

Authorization:

Role Based

Roles:

* Administrador
* Mesero
* Chef

---

# Data Access Strategy

Primary:

Stored Procedures

Secondary:

ADO.NET

Read Operations:

Entity Framework 6

Query Support:

LINQ

Rules:

Business operations must use SP.

Dashboard and reads may use EF.

---

# Transaction Strategy

Transactions required:

* Create Order
* Add Products
* Add Additional Products
* Close Order
* Change Product Status
* Register Audit

Rollback mandatory.

---

# State Management

Pedido

* Abierto
* Enviado a cocina
* En preparacion
* Parcialmente servido
* Cerrado
* Anulado

Detalle

* Ingresado
* En preparacion
* Listo para servir
* Entregado
* Anulado

Mesa

* Disponible
* Ocupada
* Inhabilitada

State definitions belong to DATABASE.md.

---

# Error Handling

Presentation

Friendly messages

Application

Business exceptions

Persistence

Database exceptions

Global

Exception filters

Logging

---

# UI Architecture

UI rules belong exclusively to:

DESIGN.md

Architecture must NOT define:

* Colors
* Typography
* Radius
* Layout spacing

Architecture only defines:

* Rendering responsibilities
* UI boundaries

---

# Scalability

Architecture must support:

* New modules
* New reports
* Additional dashboards
* Additional notifications
* Additional entities
* Additional maintenance screens

Without modifying existing modules.

---

# Architectural Goal

Build a maintainable restaurant management platform capable of supporting:

Administration

↓

Orders

↓

Kitchen

↓

Real-time communication

↓

Future growth
