# DotNet Angular Technical Assessment

This repository contains a complete technical assessment built with ASP.NET Core Web API, ASP.NET Core MVC, Angular, RxJS, JWT authentication, cookie authentication, Entity Framework Core, and SQL Server.

## Projects

### JwtAuthenticationApi

Handles user authentication and JWT token generation.

- Login endpoint
- Register endpoint
- JWT bearer token generation
- Protected test endpoints
- SQL Server persistence with Entity Framework Core
- Demo user auto-created on first login

Demo credentials:

```text
Username: fahad
Password: Password@123
```

Main endpoint:

```text
POST https://localhost:7015/api/Auth/login
```

### ProductAPI

Provides JWT-protected product endpoints.

- Create product
- Get all products
- Get product by ID
- Validates JWT tokens issued by `JwtAuthenticationApi`
- CORS enabled for Angular
- SQL Server persistence with Entity Framework Core

Endpoints:

```text
GET  https://localhost:7156/api/Product
POST https://localhost:7156/api/Product
GET  https://localhost:7156/api/Product/{id}
```

### ProductWebApp

ASP.NET Core MVC web application.

- Login through `JwtAuthenticationApi`
- Cookie authentication between browser user and MVC web app
- JWT authentication between MVC web app and `ProductAPI`
- Create and view products through the protected Product API

### DotNetAngularAssessment / AngularClient

Angular frontend application.

- Login page
- Calls `JwtAuthenticationApi` to receive JWT token
- Stores JWT token in `localStorage`
- Sends JWT token to `ProductAPI`
- Product form component
- Product list component
- Product summary component
- Shared RxJS `BehaviorSubject` updates multiple components

## Authentication Flow

### Angular To API

1. User logs in from the Angular login page.
2. Angular calls `JwtAuthenticationApi`.
3. `JwtAuthenticationApi` returns a JWT token.
4. Angular stores the token in `localStorage`.
5. Angular calls `ProductAPI` using:

```text
Authorization: Bearer <token>
```

### MVC Web App To API

1. User logs in through `ProductWebApp`.
2. `ProductWebApp` calls `JwtAuthenticationApi`.
3. `ProductWebApp` stores the JWT token in session.
4. Browser user is authenticated with cookie authentication.
5. `ProductWebApp` calls `ProductAPI` using the JWT bearer token.

## Angular Subject Requirement

Angular uses `ProductService` as shared state.

```ts
private productsSubject = new BehaviorSubject<Product[]>([]);
public products$ = this.productsSubject.asObservable();
```

Both `ProductListComponent` and `ProductSummaryComponent` subscribe to this observable. When a product is added, `ProductService` reloads products from the API and publishes the new product list, so both components update automatically.

## Database Setup

Both APIs use SQL Server. Update the SQL Server instance in `appsettings.json` if required.

`JwtAuthenticationApi/appsettings.json`:

```json
"DefaultConnection": "Server=FAHADSPC\\SQLEXPRESS;Database=DotNetAssessmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

`ProductAPI/appsettings.json`:

```json
"DefaultConnection": "Server=FAHADSPC\\SQLEXPRESS;Database=ProductAssessmentDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

Entity Framework Core migrations are included. The APIs apply migrations automatically on startup.

## Run Instructions

Run these in separate terminals.

### 1. JwtAuthenticationApi

```powershell
cd JwtAuthenticationApi
dotnet run --launch-profile https
```

Expected URL:

```text
https://localhost:7015
```

### 2. ProductAPI

```powershell
cd ProductAPI
dotnet run --launch-profile https
```

Expected URL:

```text
https://localhost:7156
```

### 3. ProductWebApp

```powershell
cd ProductWebApp
dotnet run --launch-profile https
```

Open the localhost URL shown in the terminal.

### 4. Angular Client

```powershell
cd DotNetAngularAssessment\AngularClient
npm install
npm start
```

Open:

```text
http://localhost:4200
```

## Assessment Requirements Covered

### Requirement 1

An API authenticates a client using JWT token.

Implemented by `JwtAuthenticationApi`.

### Requirement 2

A web application makes a POST call to the API and saves and retrieves product information.

Implemented by `ProductWebApp` and `ProductAPI`.

### Requirement 3

An Angular client subscribes to a subject and reflects API changes across multiple components.

Implemented by `AngularClient`, `ProductService`, `ProductListComponent`, and `ProductSummaryComponent`.

### Requirement 4

A web app authenticates the user for access to an API, with authentication between user and web app and between web app and API.

Implemented by `ProductWebApp`, `JwtAuthenticationApi`, and `ProductAPI`.

User to web app uses cookie authentication. Web app to API uses JWT bearer authentication.

## Notes For Reviewers

- The projects are split intentionally to clearly show authentication API, product API, MVC web app, and Angular client responsibilities.
- Swagger is enabled for API testing.
- JWT authentication protects Product API endpoints.
- Angular demonstrates shared RxJS state across multiple components.
- MVC demonstrates cookie authentication plus server-side JWT API calls.
