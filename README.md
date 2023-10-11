# EUniversity
An e-university Web Application built with ASP.NET and React.

## Tech Stack

### Back end
* .NET 7.0
* Entity Framework Core
* SQL Server
* Duende Identity Server
* Mapster
* FluentValidation
* AnyAscii
* Swashbuckle

### Front end
* React

## Database Scheme(draft)
```
ApplicationUser(string)
	-> FirstName
	-> LastName
	-> MiddleName?
	-> Email
	-> UserName
	-> Roles(Administrator, Student, Teacher)
	...

Course(int)
	-> Name
	-> Description
	-> Teachers
	-> Students
	-> Classes
	-> ClassesNumber

Grade(int)
	-> Name
	-> Score

CourseGrade(int)
	-> Teacher
	-> Student
	-> Grade
	-> Course
	-> Notes

Classroom(int)
	-> Name
	-> Description

Class(int)
	-> Course
	-> Teacher
	-> Students
	-> Date
	-> Classroom

ClassSchedule(int)
	-> Class
	-> Teacher?
	-> Date?
	-> Classroom?

Semester(int)
	-> Number
	-> DateFrom
	-> DateTo
	-> Courses
	-> Students
```

## Architecture

This application is built on a modular architecture that follows a three-layered design pattern. The architecture consists of three primary projects, each with its distinct responsibilities:

### Presentation Layer - EUniversity
The **EUniversity** project serves as the Presentation Layer, responsible for the web application's user interface and interaction with users. It provides endpoints, handles HTTP requests, and encapsulates various components such as controllers. This layer delivers the user-facing experience and orchestrates user interactions.
### Business Logic Layer - EUniversity.Core
The **EUniversity.Core** project represents the Business Logic Layer of the application. This layer contains the core business logic, domain models, DTOs (Data Transfer Objects), validators, and service interfaces. It defines the fundamental rules, operations, and data processing logic that drive the application's functionality.
### Data Access Layer - EUniversity.Infrastructure
The **EUniversity.Infrastructure** project serves as the Data Access Layer, housing the implementations of services, data access logic, and database-related code. It handles the technical aspects of working with databases, external services, and other infrastructure-related concerns.

## Backend Testing

Backend testing utilizes the NUnit framework in conjunction with NSubstitute for both unit and integration testing.

### Integration Tests

Integration tests are categorized into two primary groups: **Controllers** and **Services** tests. Both sets of tests make use of the `WebApplicationFactory` from `Microsoft.AspNetCore.Mvc.Testing` to facilitate testing our web application.

#### Controllers Tests
These tests focus on verifying the behavior and responses of endpoints within our application. They are an essential component of our quality assurance process, ensuring that our API endpoints function as expected. Our Controllers tests inherit from the ControllersTest class, which leverages the `MockedProgramWebApplicationFactory`. Importantly, these tests do not interact with the actual database but instead rely on mock objects to validate the behavior.

#### Services Tests
These tests are designed to assess our application's services and their interactions with the database. These tests are performed against a local database, ensuring that our services operate as intended when interfacing with real data. These tests are also compatible with GitHub Actions, where they take advantage of an in-memory database for faster test execution. Services tests are derived from the `ServicesTest` class, utilizing the `ProgramWebApplicationFactory`. To maintain test isolation and prevent cross-test contamination, transaction scope is used.

## Admin Access

Username: ```admin```

Password: ```Chang3M3InProduct10nPlz!```

## Test users

#### Student

Username: ```student```

Password: ```Password1!```

#### Teacher

Username: ```teacher```

Password: ```Password1!```
