# Database Schema — jff-csharp-tools

`jff-csharp-tools` is a **library**, not a standalone application. It does not own a database
schema. Instead, it provides base entity classes that consumer applications use to build their
own EF Core models. The library also ships a ready-to-use entity for Serilog structured logging.

---

## Base Entities

Consumer apps inherit from one of the following abstract entities to get audit fields and
built-in LINQ filtering.

---

### `DefaultEntity<TEntity>`

Namespace: `JffCsharpTools.Domain.Entity`  
Package: `jff-csharp-tools` (core)

| Column           | Type       | Constraints       | Description                                      |
|------------------|------------|-------------------|--------------------------------------------------|
| `Id`             | `int`      | PK, auto-increment | Primary key                                     |
| `CreatorUserId`  | `int`      | NOT NULL          | ID of the user who created the record (int key)  |
| `CreatedAt`      | `datetime` | NOT NULL          | Timestamp of record creation                     |
| `UpdatedAt`      | `datetime` | NULLABLE          | Timestamp of last update; NULL if never updated  |

**Usage:**
```csharp
public class Product : DefaultEntity<Product>
{
    public string Name { get; set; }
}
```

---

### `DefaultGuidEntity<TEntity>`

Namespace: `JffCsharpTools.Domain.Entity`  
Package: `jff-csharp-tools` (core)

| Column           | Type       | Constraints       | Description                                         |
|------------------|------------|-------------------|-----------------------------------------------------|
| `Id`             | `int`      | PK, auto-increment | Primary key                                        |
| `CreatorUserId`  | `uniqueidentifier` | NOT NULL | ID of the user who created the record (Guid key) |
| `CreatedAt`      | `datetime` | NOT NULL          | Timestamp of record creation                        |
| `UpdatedAt`      | `datetime` | NULLABLE          | Timestamp of last update; NULL if never updated     |

**Usage:**
```csharp
public class Order : DefaultGuidEntity<Order>
{
    public string Description { get; set; }
}
```

---

### `LogSerilogEntity`

Namespace: `JffCsharpTools.Domain.Entity`  
Package: `jff-csharp-tools` (core)

Maps to the **Serilog** sink table for structured log persistence (e.g., `AspNetApiWebAuthLogs`).
The exact table name is configured by the consumer's Serilog sink setup.

| Column            | Type       | Constraints       | Description                                             |
|-------------------|------------|-------------------|---------------------------------------------------------|
| `Id`              | `int`      | PK, auto-increment | Primary key                                            |
| `Message`         | `varchar`  | NULLABLE          | Rendered log message                                    |
| `MessageTemplate` | `varchar`  | NULLABLE          | Original Serilog message template                       |
| `Level`           | `varchar`  | NULLABLE          | Log level (e.g., `Information`, `Error`, `Warning`)     |
| `Timestamp`       | `datetime` | NOT NULL          | When the log entry was emitted                          |
| `Exception`       | `text`     | NULLABLE          | Exception message and/or stack trace                    |
| `Properties`      | `text/jsonb` | NULLABLE        | Structured properties as JSON (Serilog enrichers, etc.) |
| `LogEvent`        | `varchar`  | NULLABLE          | Log event identifier / event ID                         |

**Example Serilog sink table DDL (PostgreSQL):**
```sql
CREATE TABLE "AspNetApiWebAuthLogs" (
    "Id"              SERIAL PRIMARY KEY,
    "Message"         TEXT,
    "MessageTemplate" TEXT,
    "Level"           VARCHAR(50),
    "Timestamp"       TIMESTAMP NOT NULL,
    "Exception"       TEXT,
    "Properties"      JSONB,
    "LogEvent"        TEXT
);
```

---

## Entity Relationships

Because the library provides **base classes**, each consumer application defines its own
entity graph. The only structural relationship enforced by the library is:

```
[Consumer Entity]  ──extends──▶  DefaultEntity<TEntity>  (or DefaultGuidEntity<TEntity>)
```

All consumer entities share the audit columns (`Id`, `CreatorUserId`, `CreatedAt`, `UpdatedAt`).
Foreign-key relationships between entities are defined entirely by the consumer app.

---

## Conventions enforced by the library

| Convention                  | Detail                                                                 |
|-----------------------------|------------------------------------------------------------------------|
| Primary key type            | `int` (auto-increment) for all base entities                           |
| User tracking               | Every record stores `CreatorUserId` (int or Guid)                      |
| Soft-delete                 | Not provided by default; consumers must add their own `DeletedAt` field |
| Timestamps                  | `CreatedAt` is set on Create; `UpdatedAt` is set on Update             |
| Row-level security          | Services filter by `CreatorUserId == IdUser` when `filterCurrentUser = true` |
