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