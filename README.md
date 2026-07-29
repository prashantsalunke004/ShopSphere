# 🛒 ShopSphere - E-Commerce Backend API

A modern E-Commerce Backend API built using **ASP.NET Core Web API** following clean architecture principles and industry best practices.

The project demonstrates backend development concepts including RESTful APIs, Entity Framework Core, Repository Pattern, Dependency Injection, Unit Testing, and Continuous Integration using GitHub Actions.

---

## 🚀 Features

- Product Management (CRUD)
- Category Management (CRUD)
- RESTful APIs
- Entity Framework Core
- Repository Pattern
- Dependency Injection
- DTO Pattern
- LINQ Queries
- SQL Server Database
- Async/Await
- Unit Testing using xUnit & Moq
- GitHub Actions CI Pipeline
- Swagger API Documentation

---

## 🛠 Tech Stack

| Technology | Description |
|------------|-------------|
| ASP.NET Core Web API | Backend Framework |
| C# | Programming Language |
| Entity Framework Core | ORM |
| SQL Server | Database |
| LINQ | Data Querying |
| Dependency Injection | Design Pattern |
| Repository Pattern | Data Access Layer |
| xUnit | Unit Testing |
| Moq | Mocking Framework |
| Git | Version Control |
| GitHub Actions | Continuous Integration |

---

## 📁 Project Structure

```
ShopSphere
│
├── ShopSphere.Api
│
├── ShopSphere.Core
│
├── ShopSphere.Infrastructure
│
├── ShopSphere.Tests
│
└── README.md
```

---

## 📌 API Modules

### Categories

- Get All Categories
- Get Category By Id
- Create Category
- Update Category
- Delete Category

### Products

- Get All Products
- Get Product By Id
- Create Product
- Update Product
- Delete Product

---

## 🧪 Unit Testing

Implemented unit tests using:

- xUnit
- Moq

Tests cover:

- Product Service
- Category Service
- Business Logic

---

## ⚙️ CI Pipeline

GitHub Actions automatically performs:

- Restore NuGet Packages
- Build Solution
- Execute Unit Tests
- Publish Application
- Upload Build Artifact

---

## 📷 API Documentation

Swagger UI is enabled.

```
https://localhost:5001/swagger
```

---

## 💻 Getting Started

### Clone Repository

```bash
git clone https://github.com/prashantsalunke004/ShopSphere.git
```

### Navigate

```bash
cd ShopSphere
```

### Restore Packages

```bash
dotnet restore
```

### Build

```bash
dotnet build
```

### Run

```bash
dotnet run --project ShopSphere.Api
```

---

## 🔥 Upcoming Features

- JWT Authentication
- Role Based Authorization
- Refresh Tokens
- Global Exception Handling
- Serilog Logging
- Pagination
- Filtering
- Sorting
- Search API
- File Upload
- Azure Deployment
- Docker Support
- Redis Caching

---

## 👨‍💻 Author

**Prashant Salunke**

.NET Backend Developer

LinkedIn:
(Add your LinkedIn profile)

GitHub:
https://github.com/prashantsalunke004

---

## ⭐ Support

If you found this project useful, consider giving it a ⭐ on GitHub.
