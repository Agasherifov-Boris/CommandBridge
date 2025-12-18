# Mediator for .NET

A lightweight and extensible implementation of the **Mediator pattern** for .NET, designed for clean application architecture, CQRS-style workflows, and high performance.

---

## Features

- Commands and queries separation (CQRS-friendly)
- Strongly typed handlers
- Asynchronous execution (`Task` / `ValueTask`)
- Pipeline with interceptors
- Minimal allocations
- Easy integration with Dependency Injection
- .NET 8 / .NET 9 support

---

## Installation

```bash
dotnet add package CommandBridge

