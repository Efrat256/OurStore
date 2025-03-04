# Web API Project - .NET 8

## Overview
This project is a Web API built with .NET 8, following REST API architecture. It is designed with scalability in mind and is divided into several layers to promote reusability, logical, and business separation. The main layers in this project are Repository, Service, and Entities. The project also utilizes Data Transfer Objects (DTOs) via Records, Dependency Injection for layer communication, and async/await for asynchronous programming.

## Project Layers

### Repository Layer
The Repository layer is responsible for data access. It contains the logic to interact with the database. This layer abstracts the data access logic from the rest of the application, promoting a separation of concerns.

### Service Layer
The Service layer contains the business logic of the application. It communicates with the Repository layer to perform operations on the data and applies the necessary business rules. This layer ensures that the business logic is kept separate from the data access logic.

### Entities Layer
The Entities layer contains the definitions of the data models used throughout the application. These models represent the structure of the data and are used to transfer data between the different layers of the application.

## Data Transfer Objects (DTOs) via Records
In this project, DTOs are implemented using C# Records. using Records for several advantages: Immutability, Value Equality, Conciseness.

## Dependency Injection
Dependency Injection (DI) is used in this project to manage the dependencies between the layers. DI provides several benefits:Decoupling, Maintainability, Testability.

## Async/Await
The project uses async/await for asynchronous programming. The advantages of async/await in .NET include:Improved Performance, Simplified Code, Scalability.

## Database
The project uses a SQL database, connected via Entity Framework ORM with a Code-First approach. This allows the database schema to be created and updated based on the C# model classes.

## API Documentation
The project includes a list of APIs which can be accessed through Swagger for testing and documentation purposes. The Swagger UI provides a user-friendly interface to interact with the API endpoints.
link to swagger:https://localhost:44336/swagger/index.html.

## AutoMapper
The project uses AutoMapper for converting between the different layers. AutoMapper maps properties between objects, reducing the need for manual mapping code and minimizing errors.

## Configuration
The project includes a configuration file with runtime settings, including the connection string for the database. This allows for easy configuration and switching between different environments (e.g., development, testing, production).

## Error Handling
Error handling in the project is managed via middleware. The middleware logs errors and, in case of critical errors, sends an email notification. This ensures that errors are properly tracked and managed, improving the reliability of the application.

## Request Tracking
All requests to the API are logged in a rating table for monitoring and analysis purposes. This helps in identifying usage patterns and potential issues.

## Scalability Considerations
The project is designed with scalability in mind. The use of layers promotes separation of concerns, making it easier to manage and scale the application. The use of async/await ensures that the application can handle a large number of concurrent requests efficiently.

## Conclusion
This .NET 8 Web API project follows best practices in software architecture, using layers for separation of concerns, records for concise and immutable data transfer objects, dependency injection for managing dependencies, and async/await for efficient asynchronous programming. The design choices made in this project aim to provide a scalable, maintainable, and testable codebase.