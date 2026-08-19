<div align="center">

# 🏠 Rafeek RoomMate API

### A Scalable Roommate & Shared Housing Backend

**Designed & Developed by Tarek Elabsy**

<br/>

[![.NET](https://img.shields.io/badge/.NET-ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-Backend-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/dotnet/csharp/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Database-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![EF Core](https://img.shields.io/badge/EF%20Core-ORM-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-black?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)

<br/>

**Clean Architecture • CQRS • REST API • JWT Authentication • PostgreSQL**

</div>

---

## 📌 About The Project

**Rafeek RoomMate API** is a backend RESTful API for a roommate and shared-housing platform.

The system is designed to help users create accounts, manage their profiles, securely authenticate, upload profile images, and provide a foundation for future roommate matching and housing-related features.

The project focuses on building a maintainable and scalable backend using modern **.NET development practices**.

The architecture separates business rules, application use cases, infrastructure, and HTTP concerns to keep the system flexible as the project grows.

---

## 🎯 Project Goals

The main goals of Rafeek RoomMate API are:

- Build a scalable backend for a roommate platform.
- Provide secure authentication and authorization.
- Separate business logic from infrastructure concerns.
- Follow Clean Architecture principles.
- Organize application features using CQRS.
- Use PostgreSQL for reliable relational data storage.
- Make the system easy to extend with future features.
- Maintain a clear separation between Identity and user profile data.
- Provide reusable abstractions for external services such as file storage.

---

# 🏗️ Architecture

The solution follows **Clean Architecture** with four main layers:

```text
                    ┌─────────────────────────┐
                    │      Rafeek RoomMate    │
                    │          API            │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │      Application        │
                    │                         │
                    │ Commands / Queries      │
                    │ Use Cases / Interfaces  │
                    └────────────┬────────────┘
                                 │
                                 ▼
                    ┌─────────────────────────┐
                    │         Domain          │
                    │                         │
                    │ Entities / Rules        │
                    │ Value Objects           │
                    └─────────────────────────┘
                                 ▲
                                 │
                    ┌────────────┴────────────┐
                    │     Infrastructure      │
                    │                         │
                    │ EF Core / PostgreSQL    │
                    │ Identity / Storage      │
                    └─────────────────────────┘
