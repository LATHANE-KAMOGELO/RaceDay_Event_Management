# RaceDay – Event Management System

**Author:** Lathane Kamogelo

[![RaceDay API CI](https://github.com/LATHANE-KAMOGELO/RaceDay_Event_Management/actions/workflows/dotnet.yml/badge.svg?branch=part-2-restful-api)](https://github.com/LATHANE-KAMOGELO/RaceDay_Event_Management/actions/workflows/dotnet.yml)

## Project Overview

RaceDay is a web-based event management system for the South African road running, walking, and cycling community. It is designed to help event organisers manage events, categories, routes, participant enrolments, and race results. Participants can create accounts, browse events, enrol in races, and view their results.

The project is developed in stages. Part 1 covers system planning and database design. Part 2 provides the RESTful API. Part 3 is planned to add the user-facing MVC application and further deployment features.

## System Roles

### Organiser

An Organiser manages race information. Depending on the implemented API permissions, Organisers can:

- Create, view, edit, and delete events.
- Create and manage event categories and routes.
- View and manage participant enrolments.
- Record and manage race results.
- Manage user information where authorised.

### Participant

A Participant takes part in RaceDay events. Participants can:

- Register an account and log in.
- Browse event information.
- Enrol in an event and category.
- View race information and results where the API permits.

## Part 1 – System Planning and Database

Part 1 establishes the system design and database foundation. Planning materials include:

- Entity Relationship Diagram (ERD)
- API Endpoint Plan
- SQL Server database script
- Fictional sample data for demonstration and testing

The database is designed to store users, events, categories, routes, enrolments, and results. Primary keys identify records, and foreign keys link related data.

### Part 1 Video Presentation

[Watch the RaceDay Part 1 presentation on YouTube](https://youtu.be/SPbgf3eZTow)

## Repository Structure

```text
RaceDay_Event_Management/
├── docs/
│   ├── RaceDay-ERD.png
│   ├── RaceDay-Endpoint-Plan.md
│   ├── RaceDay.sql
│   └── Seed-Data-Note.md
├── .github/
│   └── workflows/
│       ├── validate-docs.yml
│       └── dotnet.yml
├── RaceDayAPI/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Migrations/
│   ├── Properties/
│   ├── appsettings.json
│   └── Program.cs
├── RaceDayAPI.Tests/
│   └── UnitTest1.cs
└── README.md
```

The exact files in the repository may change as development continues.

## Part 2 – RESTful API

Part 2 implements the RaceDay REST API using ASP.NET Core Web API and **.NET 10**. Entity Framework Core connects the API to Microsoft SQL Server.

### API Features

- CRUD operations for users, events, categories, routes, enrolments, and results.
- User registration and login using JWT bearer tokens.
- Role-based access for Organisers and Participants.
- Swagger/OpenAPI documentation and interactive API testing.
- Entity Framework Core with SQL Server.
- An API endpoint to check database connectivity.
- Automated tests and GitHub Actions CI when the test project and workflow are present and configured.

### Running the API

1. Install Visual Studio with the .NET 10 SDK and ensure SQL Server Express is installed and running.
2. Open `RaceDay_Event_Management.slnx` in Visual Studio.
3. In `RaceDayAPI/appsettings.json`, check that the connection string points to your SQL Server instance and database. The local server instance used for this project is `KAMOGELO\\SQLEXPRESS`.
4. Make sure the `RaceDay_Event_Management` database and required tables exist in SQL Server. Use the SQL script or Entity Framework migrations according to the project setup.
5. Set `RaceDayAPI` as the startup project.
6. Build the solution, then press **F5** or click the green **https** run button.
7. Open the Swagger URL printed when the API starts. The current local URL used during development is [https://localhost:7158/swagger/index.html](https://localhost:7158/swagger/index.html). The port may differ on your computer.
8. Use the Auth register and login endpoints. Copy the login token into Swagger's **Authorize** dialog to test protected endpoints.

### Database Connection

The API uses SQL Server Express and Windows Authentication in the local development connection string. A typical connection string is:

```text
Server=KAMOGELO\\SQLEXPRESS;Database=RaceDay_Event_Management;Trusted_Connection=True;TrustServerCertificate=True;
```

If your SQL Server instance name is different, replace the `Server=` value with the instance shown in SSMS. Keep passwords, signing keys, and other secrets out of public source control.

### API Endpoints

The API provides operations for these resources. The Swagger page is the authoritative source for the current routes, request fields, and authorization requirements.

| Resource | Main operations |
|---|---|
| Authentication | Register and log in |
| Users | Create, view, update, and delete users |
| Events | Create, view, update, and delete events |
| Categories | Create, view, update, and delete event categories |
| Routes | Create, view, update, and delete race routes |
| Enrolments | Create, view, update, and delete participant enrolments |
| Results | Create, view, update, and delete race results |
| Database check | Check whether the API can connect to SQL Server |

Swagger operations generally use `GET` to read, `POST` to create, `PUT` to update, and `DELETE` to remove records. Follow the role requirements shown by the API; some operations require an authenticated Organiser or Participant.

### Unit Testing

If the `RaceDayAPI.Tests` project is included in the solution, it contains automated xUnit tests. Tests using the EF Core InMemory provider can run without changing the SQL Server database.

From the repository root, run:

```text
dotnet test RaceDayAPI.Tests/RaceDayAPI.Tests.csproj
```

The test project and tests must exist for this command to succeed.

### Continuous Integration

The repository includes GitHub Actions workflow files under `.github/workflows`. The workflows can validate documentation and, when configured, restore dependencies, build the API, and run automated tests when changes are pushed to GitHub. The CI badge at the top links to the repository's `dotnet.yml` workflow on the `part-2-restful-api` branch.

## Part 1 Database Documentation

The `/docs` folder is intended to contain the planning and database documentation:

- **ERD:** Shows entities, attributes, primary keys, foreign keys, and relationships.
- **API Endpoint Plan:** Describes HTTP methods, routes, purposes, role requirements, request bodies, and expected responses.
- **SQL Database Script:** Defines the database schema, constraints, and sample data.
- **Seed Data Note:** Describes the fictional sample records.

The SQL database script can be opened and executed in SQL Server Management Studio (SSMS). After running it, use verification queries to inspect the tables and sample records.

## Part 1 Progress

Update this checklist to reflect the files and evidence actually present in the repository.

- [x] ERD created
- [x] API endpoint plan created
- [x] SQL Server database script created
- [x] Database sample data added
- [x] SQL script tested in SSMS
- [x] Project documentation added
- [x] GitHub Actions validation workflow added
- [x] Part 1 video presentation linked

## Future Development

Part 3 is planned to add an MVC web application that consumes the REST API and provides a graphical interface for Organisers and Participants. Additional planned features may include Azure Blob Storage and Docker containerisation.

## GitHub Repository

The source code and project documentation are hosted here:

[RaceDay Event Management on GitHub](https://github.com/LATHANE-KAMOGELO/RaceDay_Event_Management)

