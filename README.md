# RaceDay Event Management API

An ASP.NET Core Web API for managing race events, categories, routes, participant enrolments, results, and user accounts. It stores data in SQL Server and provides interactive API documentation through Swagger UI.

## Technology

- .NET 10 / ASP.NET Core Web API
- Entity Framework Core with SQL Server
- JWT bearer authentication and role-based authorization
- Swagger UI
- SQL Server Express database: `RaceDay_Event_Management`

## Project layout

RaceDayAPI/
├── Controllers/       API endpoint controllers
├── Data/              RaceDayContext database context
├── Models/            Category, Enrolment, Event, Result, Route, User
├── appsettings.json   SQL Server connection string
└── Program.cs         Services, JWT, Swagger, and HTTP pipeline
