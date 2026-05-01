# Main Flows — jff-csharp-tools

This document describes the primary runtime flows that occur when a consumer application uses
the `jff-csharp-tools` library.

---

## 1. Request Authentication Flow

Applies to any endpoint that inherits from `DefaultController` (MVC) or uses
`HttpContextExtension.CurrentUserId()` (Minimal API).

```plantuml
@startuml Authentication Flow
participant "HTTP Client" as Client
participant "ASP.NET Core Pipeline" as Pipeline
participant "TokenEnumFilter" as TokenFilter
participant "DefaultController\n/ HttpContextExtension" as Controller
participant "JWT Bearer Middleware" as JWT

Client -> Pipeline: HTTP Request\n(Bearer token in header / query / cookie)
Pipeline -> JWT: ValidateToken()
alt Token valid
    JWT --> Pipeline: ClaimsPrincipal set
    Pipeline -> TokenFilter: OnActionExecuting()
    TokenFilter -> TokenFilter: Reads token via\nTokenParameterEnum\n(Header | Query | Cookie)
    TokenFilter --> Pipeline: Continue
    Pipeline -> Controller: Execute action
    Controller -> Controller: CurrentIdUser_FromBearerToken\n(reads "sub" / user claim)
    Controller --> Client: 200 OK + response body
else Token invalid or missing
    JWT --> Pipeline: 401 Unauthorized
    Pipeline --> Client: 401 Unauthorized
end
@enduml
```

### Step-by-step

1. Client sends an HTTP request with a Bearer JWT token.
2. ASP.NET Core JWT Bearer middleware validates the token signature and expiry.
3. `TokenEnumFilter` (when registered) validates that the token is present in the configured
   location (header, query string, or cookie).
4. `DefaultController.CurrentIdUser_FromBearerToken` reads the user ID from JWT claims.
5. All service calls use this ID for row-level filtering.

---

## 2. Create Entity Flow (POST)

```plantuml
@startuml Create Entity Flow
participant "HTTP Client" as Client
participant "DefaultCRUDController\n(or CrudEndpoints)" as Controller
participant "DefaultService<T>" as Service
participant "DefaultRepository<T>" as Repository
participant "DbContext (EF Core)" as DB

Client -> Controller: POST /api/resource\n{ entity JSON body }
Controller -> Controller: CurrentIdUser_FromBearerToken
Controller -> Service: Create(IdUser, entity)
Service -> Service: entity.CreatorUserId = IdUser\nentity.CreatedAt = UtcNow
Service -> Repository: Add(entity)
Repository -> DB: DbSet<TEntity>.Add(entity)
Service -> Repository: SaveChanges()
Repository -> DB: INSERT INTO table
DB --> Repository: entity.Id (auto-generated)
Repository --> Service: entity.Id
Service --> Controller: Result<int> { Result = Id }
Controller -> Controller: ReturnAction() / ReturnResult()
Controller --> Client: 200 OK { result: <new Id> }
@enduml
```

---

## 3. Read Entity Flow (GET by user)

```plantuml
@startuml Read Entities Flow
participant "HTTP Client" as Client
participant "DefaultCRUDController\n(or CrudEndpoints)" as Controller
participant "DefaultService<T>" as Service
participant "DefaultRepository<T>" as Repository
participant "DbContext (EF Core)" as DB

Client -> Controller: GET /api/resource
Controller -> Service: GetByUser<TEntity>(IdUser)
Service -> Repository: Get<TEntity>(filter: x => x.CreatorUserId == IdUser)
Repository -> DB: SELECT * FROM table\nWHERE CreatorUserId = IdUser
DB --> Repository: IEnumerable<TEntity>
Repository --> Service: IEnumerable<TEntity>
Service --> Controller: Result<IEnumerable<TEntity>>
Controller --> Client: 200 OK [ ... entities ... ]
@enduml
```

**Note:** When `filterCurrentUser = false`, the service calls `Get<TEntity>()` without the
`CreatorUserId` filter, returning all records.

---

## 4. Paginated Query Flow (GET /pagination)

```plantuml
@startuml Paginated Query Flow
participant "HTTP Client" as Client
participant "DefaultCRUDController" as Controller
participant "DefaultService<T>" as Service
participant "DefaultRepository<T>" as Repository
participant "DbContext (EF Core)" as DB

Client -> Controller: GET /api/resource/pagination\n?Page=2&CountPerPage=10&Order=Name
Controller -> Service: GetPaginated(paginationModel,\n filter: f => true, IdUser)
Service -> Service: Apply CreatorUserId filter\nif filterCurrentUser == true
Service -> Repository: GetPaginated(query, skip, take, order)
Repository -> DB: SELECT COUNT(*) ...
Repository -> DB: SELECT * ... OFFSET skip LIMIT take ORDER BY Name
DB --> Repository: (total, IEnumerable<TEntity>)
Repository --> Service: PaginationResult<TEntity>\n{ List, Total, TotalPages }
Service --> Controller: Result<PaginationResult<TEntity>>
Controller --> Client: 200 OK { list: [...],\n total: 50, page: 2, totalPages: 5 }
@enduml
```

---

## 5. Update Entity Flow (PUT /{key})

```plantuml
@startuml Update Entity Flow
participant "HTTP Client" as Client
participant "DefaultCRUDController\n(or CrudEndpoints)" as Controller
participant "DefaultService<T>" as Service
participant "DefaultRepository<T>" as Repository
participant "DbContext (EF Core)" as DB

Client -> Controller: PUT /api/resource/{key}\n{ updated entity JSON }
Controller -> Service: UpdateByKey(IdUser, entity, key)
Service -> Repository: GetByKey<TEntity>(key)
Repository -> DB: SELECT * FROM table WHERE Id = key
DB --> Repository: existing entity (or null)
alt Entity found and belongs to user (or filterCurrentUser=false)
    Service -> Service: Map updated fields\nentity.UpdatedAt = UtcNow
    Service -> Repository: Update(entity)
    Repository -> DB: UPDATE table SET ... WHERE Id = key
    Service -> Repository: SaveChanges()
    Service --> Controller: Result<bool> { Result = true }
    Controller --> Client: 200 OK { result: true }
else Entity not found or not owned by user
    Service --> Controller: Result<bool>\n{ StatusCode = 404/403 }
    Controller --> Client: 404 Not Found / 403 Forbidden
end
@enduml
```

---

## 6. Delete Entity Flow (DELETE /{key})

```plantuml
@startuml Delete Entity Flow
participant "HTTP Client" as Client
participant "DefaultCRUDController\n(or CrudEndpoints)" as Controller
participant "DefaultService<T>" as Service
participant "DefaultRepository<T>" as Repository
participant "DbContext (EF Core)" as DB

Client -> Controller: DELETE /api/resource/{key}
Controller -> Service: DeleteByKey<TEntity, int>(IdUser, key)
Service -> Repository: GetByKey<TEntity>(key)
Repository -> DB: SELECT * FROM table WHERE Id = key
DB --> Repository: entity (or null)
alt Entity found and belongs to user
    Service -> Repository: Delete(entity)
    Repository -> DB: DELETE FROM table WHERE Id = key
    Service -> Repository: SaveChanges()
    Service --> Controller: Result<bool> { Result = true }
    Controller --> Client: 200 OK { result: true }
else Not found or not owned
    Service --> Controller: Result<bool>\n{ StatusCode = 404/403 }
    Controller --> Client: 404 / 403
end
@enduml
```

---

## 7. Exception Handling Flow

Applies globally when `ExceptionFilter` is registered in the application.

```plantuml
@startuml Exception Handling Flow
participant "HTTP Client" as Client
participant "ASP.NET Core Pipeline" as Pipeline
participant "Controller / Endpoint" as Controller
participant "ExceptionFilter" as Filter
participant "ILogger" as Logger

Client -> Pipeline: HTTP Request
Pipeline -> Controller: Execute action
Controller --> Pipeline: throws Exception
Pipeline -> Filter: OnException(ExceptionContext)
alt UnauthorizedAccessException / TokenException
    Filter -> Logger: LogWarning(Unauthorized_System)
    Filter --> Pipeline: 401 Unauthorized\n{ message: "Unauthorized access." }
else SmtpException
    Filter -> Logger: LogCritical(Smtp_Exception_System)
    Filter --> Pipeline: 424 Failed Dependency\n{ message: "Email sending failure." }
else FileNotFoundException
    Filter -> Logger: LogError(File_NotFound_System)
    Filter --> Pipeline: 415 Unsupported Media Type\n{ message: "File not found." }
else DbException
    Filter -> Logger: LogCritical(DB_Exception_System)
    Filter --> Pipeline: 424 Failed Dependency\n{ message: "Database failure." }
else Other exceptions
    Filter -> Logger: LogError(Generic_Exception_System)
    Filter --> Pipeline: 500 Internal Server Error\n{ message: "An unexpected error occurred." }
end
Pipeline --> Client: Standardized error response\n(with Error/StackTrace fields in DEBUG mode)
@enduml
```

---

## 8. Unit of Work Flow

When `UnitOfWorkFilter` is registered, `SaveChanges()` is called automatically after every
successful action, eliminating the need to call it manually in every service method.

```plantuml
@startuml Unit of Work Flow
participant "HTTP Client" as Client
participant "ASP.NET Core Pipeline" as Pipeline
participant "UnitOfWorkFilter" as UoW
participant "Controller / Service" as Service
participant "DbContext (EF Core)" as DB

Client -> Pipeline: HTTP Request
Pipeline -> UoW: OnActionExecuting()
UoW --> Pipeline: Continue
Pipeline -> Service: Execute action\n(Add / Update / Delete entities)
Service --> Pipeline: Action result
Pipeline -> UoW: OnActionExecuted()
UoW -> DB: SaveChanges()
DB --> UoW: OK
UoW --> Pipeline: Continue
Pipeline --> Client: HTTP Response
@enduml
```
