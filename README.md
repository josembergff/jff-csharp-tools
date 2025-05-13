# C# Tools
C# Tools for .NET Core version 6.0.36, 8.0.11 or 9 is an open-source project offering a suite of utilities to enhance C# development for .NET Core. It includes libraries for common tasks, code snippets, and performance optimizations, helping developers improve productivity, code quality, and simplify complex tasks.

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
var pagination = new PaginationModel<Product, DefaultFilter<Product>>
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