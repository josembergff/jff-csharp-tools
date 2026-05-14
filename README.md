# jff-csharp-tools

**jff-csharp-tools** is an open-source NuGet library suite for **.NET Core** (versions 6, 8 and 9) that provides reusable
utilities to accelerate C# API development. It ships opinionated base classes and helpers for the most common
boilerplate patterns: base entities with audit fields, generic repository/service layers, ready-made CRUD controllers
and Minimal API endpoints, global exception handling, JWT authentication helpers, and a rich set of domain extensions.

📖 **Documentation:** [`/docs`](./docs)

🏛️ **Arquitetura (PT-BR):** [`/docs/architecture/README.md`](./docs/architecture/README.md)

## Install Package Manager

```bash
PM> Install-Package jff_csharp-tools-6
```
or
```bash
PM> Install-Package jff_csharp-tools-8
```
or
```bash
PM> Install-Package jff_csharp-tools-9
```

## Install .NET CLI
```bash
> dotnet add package jff_csharp-tools-6
```
or
```bash
> dotnet add package jff_csharp-tools-8
```
or
```bash
> dotnet add package jff_csharp-tools-9
```

## Install Paket CLI

```bash
> paket add jff_csharp-tools-6
```
or
```bash
> paket add jff_csharp-tools-8
```
or
```bash
> paket add jff_csharp-tools-9
```

## Example of use in a .NET API project

### Example 1: Using default entities

```csharp
using Jff.CSharpTools.Domain.Entity;

public class MyEntity : DefaultEntity
{
    public string Name { get; set; }
}
```

> Namespaces may vary depending on the package version (6, 8, or 9). Adjust the namespace according to the package installed in your project.

## Example 2: Using DefaultService

```csharp
using JffCsharpTools8.Domain.Service;
using JffCsharpTools8.Domain.Repository;
using JffCsharpTools.Domain.Entity;
using JffCsharpTools.Domain.Filters;
using JffCsharpTools.Domain.Model;
using Microsoft.EntityFrameworkCore;

// Suppose you have an entity:
public class Product : DefaultEntity<Product>
{
    public string Name { get; set; }
}

// And a DbContext:
public class MyDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
}

// Instantiating the service (dependency injection recommended):
var repository = new DefaultRepository<MyDbContext>(/* parameters */);
var service = new DefaultService<MyDbContext>(repository);

// Creating a new product
var newProduct = new Product { Name = "T-shirt" };
var createResult = await service.Create<Product>(userId, newProduct);

// Getting all products
var products = await service.Get<Product>();

// Getting products by user
var userProducts = await service.GetByUser<Product>(userId);

// Getting products by filter
var filter = new DefaultFilter<Product> { /* set filters */ };
var filteredProducts = await service.GetByFilter<Product, DefaultFilter<Product>>(filter);

// Getting product by primary key
var product = await service.GetByKey<Product, int>(userId, productId);

// Paginating products
var pagination = new PaginationModel<Product>
{
    Page = 1,
    PageSize = 10,
    Filter = new DefaultFilter<Product>()
};
var paginatedProducts = await service.GetPaginated<Product>(pagination, x => x.Name != null);

// Updating a product
newProduct.Name = "Updated T-shirt";
var updateResult = await service.UpdateByKey<Product, int>(userId, newProduct, productId);

// Deleting a product
var deleteResult = await service.DeleteByKey<Product, int>(userId, productId);
```

> Adapt the examples according to the package version (6, 8 or 9) and the namespaces used in your project.

---

## Technology Stack

| Component              | Technology                              |
|------------------------|-----------------------------------------|
| Language               | C# (.NET Standard 2.1 / .NET 6, 8, 9)  |
| ORM                    | Entity Framework Core (6 / 8 / 9)      |
| HTTP framework         | ASP.NET Core MVC + Minimal APIs         |
| Authentication         | ASP.NET Core JWT Bearer                 |
| Logging entity         | Serilog (LogSerilogEntity)              |
| CSV support            | CsvHelper                               |
| Package registry       | NuGet.org                               |

---

## Principal Modules

| Module / Package                  | Description                                                                             |
|-----------------------------------|-----------------------------------------------------------------------------------------|
| `jff-csharp-tools` (core)         | Shared entities, models, filters, and extension helpers (netstandard2.1)                |
| `Domain/Entity`                   | `DefaultEntity<T>`, `DefaultGuidEntity<T>`, `LogSerilogEntity`                         |
| `Domain/Model`                    | `DefaultResponseModel<T>`, `PaginationModel<T>`, `FileModel`, `ChartModel`, `TypeModel` |
| `Domain/Filters`                  | `DefaultFilter<T>`, `DefaultEntityFilter`, `PredicateBuilderFilter`                     |
| `Domain/Extensions`               | `StringExtension`, `DateTimeExtension`, `EnumExtension`, `LinqExtensions`, `ClassExtension`, `EnumerableExtension` |
| `jff-csharp-tools-6/8/9`          | Versioned packages with repository, service, controllers, and filters                   |
| `Domain/Repository`               | `DefaultRepository<T>`, `DefaultGuidRepository<T>` (EF Core)                          |
| `Domain/Service`                  | `DefaultService<T>`, `DefaultGuidService<T>`                                           |
| `Apresentation/Controllers`       | `DefaultController`, `DefaultCRUDController<TService,TContext,TEntity>`                |
| `Apresentation/Endpoints` (v9)    | `CrudEndpoints`, `CrudGuidEndpoints` (Minimal API helpers)                             |
| `Apresentation/Filters`           | `ExceptionFilter`, `TokenEnumFilter`, `UnitOfWorkFilter`                               |
| `Apresentation/Extensions`        | `HttpContextExtension`, `DefaultResponseModelExtension`                                |

---

## How to Build Locally

### Prerequisites

- [.NET SDK 9](https://dotnet.microsoft.com/download) (also supports .NET 6 and 8)
- Git

### Clone and restore

```bash
git clone https://github.com/josembergff/jff-csharp-tools.git
cd jff-csharp-tools
dotnet restore
```

### Build all projects

```bash
dotnet build jff-csharp-tools.sln
```

### Build a specific package

```bash
dotnet build jff-csharp-tools-9/jff-csharp-tools-9.csproj
```

### Pack NuGet packages

```bash
dotnet pack jff-csharp-tools-9/jff-csharp-tools-9.csproj --configuration Release
```

Or use the provided release script:

```bash
bash run-releaseBuild.sh
```

---

## Documentation

Full technical documentation is located in [`/docs`](./docs):

| File | Description |
|------|-------------|
| [`/docs/architecture/context.puml`](./docs/architecture/context.puml) | C4 Context diagram (PlantUML) |
| [`/docs/architecture/containers.puml`](./docs/architecture/containers.puml) | C4 Containers diagram (PlantUML) |
| [`/docs/architecture/components.puml`](./docs/architecture/components.puml) | C4 Components diagram (PlantUML) |
| [`/docs/database/schema.md`](./docs/database/schema.md) | Base entity schema and conventions |
| [`/docs/flows/main-flows.md`](./docs/flows/main-flows.md) | Main runtime flows (auth, CRUD, pagination, exception handling) |
