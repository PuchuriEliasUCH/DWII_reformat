# DATABASE.md

# AltaMesa — Database Rules

## Database Engine

Microsoft SQL Server 2022

Database:

AltaMesaDB

---

# Database Philosophy

Database is responsible for:

* Persistence
* Data integrity
* Referential consistency
* Transaction execution
* Business operation orchestration
* Historical traceability

Application logic must complement the database.

Business rules must not be duplicated.

---

# Naming Convention

Database naming follows:

snake_case

Examples:

usuario

pedido

detalle_pedido

categoria_producto

---

# Primary Keys

Pattern:

id_<entity>

Examples:

id_usuario

id_pedido

id_producto

id_mesa

All PK:

* INT
* IDENTITY(1,1)

---

# Foreign Keys

Pattern:

id_<referenced_entity>

Examples:

id_rol

id_categoria

id_mesero

All relationships must define:

* FK
* Referential integrity

Orphan records are forbidden.

---

# Audit Standards

Transactional tables should contain:

created_at

updated_at

updated_by

estado

When applicable.

Examples:

usuario

producto

pedido

detalle_pedido

---

# Status Strategy

Physical deletion is forbidden.

Use:

estado

Examples:

estado = 1

estado = 0

Soft delete preferred.

---

# Tables

Core entities:

rol

usuario

mesa

categoria_producto

producto

pedido

detalle_pedido

auditoria_estado_detalle_pedido

---

# Relationship Rules

Required:

PK

FK

CHECK

UNIQUE

Business validations

Examples:

pedido
→ mesa

pedido
→ usuario

detalle_pedido
→ producto

detalle_pedido
→ pedido

---

# State Definitions

## Pedido

Allowed:

* Abierto
* Enviado a cocina
* En preparacion
* Parcialmente servido
* Cerrado
* Anulado

---

## Mesa

Allowed:

* Disponible
* Ocupada
* Inhabilitada

---

## Detalle Pedido

Allowed:

* Ingresado
* En preparacion
* Listo para servir
* Entregado
* Anulado

Never invent new states.

---

# Stored Procedures

Pattern:

sp_<action>_<entity>

Examples:

sp_login

sp_crear_usuario

sp_crear_pedido

sp_agregar_detalle_pedido

sp_agregar_adicional_pedido

sp_cerrar_pedido

sp_listar_cola_cocina

sp_cambiar_estado_detalle

sp_historial_estado_detalle

---

# Stored Procedure Categories

## CRUD

Usuario

Mesa

Categoria

Producto

---

## Authentication

sp_login

---

## Business

sp_crear_pedido

sp_agregar_detalle_pedido

sp_agregar_adicional_pedido

sp_cerrar_pedido

---

## Kitchen

sp_listar_cola_cocina

sp_cambiar_estado_detalle

sp_productos_listos

---

## Audit

sp_registrar_auditoria_estado

sp_historial_estado_detalle

---

# SQL Functions

Naming:

fn_<purpose>

Current functions:

fn_calcular_total_pedido

fn_productos_pendientes

Functions must:

* Be deterministic when possible
* Avoid side effects
* Avoid transactions

---

# Views

Naming:

vw_<purpose>

Current views:

vw_pedidos_activos

vw_cola_cocina

vw_historial_pedidos

Views must:

* Never update data
* Avoid SELECT *

---

# Entity Framework

Version:

Entity Framework 6

Allowed:

DbContext

DbSet

LINQ

Lambda

Use for:

* Queries
* Dashboard
* Read operations

Avoid:

Complex business logic

SP execution from controllers

---

# ADO.NET

Required Components

SqlConnection

SqlCommand

SqlDataReader

SqlTransaction

SqlParameter

Use for:

Stored Procedures

Transactions

Critical operations

---

# Transactions

Mandatory for:

Order creation

Additional products

Order closing

State changes

Audit registration

Rollback required on failure.

---

# Query Standards

Forbidden:

SELECT *

Dynamic SQL

String concatenation

Allowed:

Explicit columns

Parameters

Stored Procedures

Example:

SELECT
id_usuario,
nombre_usuario
FROM usuario

---

# Security

Mandatory:

SQL Parameters

Stored Procedures

Transactions

Hash passwords

Validate inputs

Forbidden:

SQL injection

Plain text passwords

Dynamic SQL

Example:

❌

"SELECT * FROM usuario WHERE id=" + id

✅

SqlParameter parameter =
new SqlParameter(
"@id",
id
);

---

# Performance Rules

Prefer:

Indexes

Views

SP

Avoid:

Nested queries

Repeated reads

Large result sets

---

# Backup Strategy

Development

Weekly

Production

Daily

Retention

30 Days

---

# Integration Rules

Database access flow:

Controller
→ Service
→ Repository
→ SQL

Controllers never access SQL.

Repositories own persistence.

---

# Source Of Truth

Database definitions override:

ARCHITECTURE.md

DESIGN.md

AGENTS.md
