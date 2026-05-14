# Arquitetura do Projeto — jff-csharp-tools

## Objetivo

Este documento descreve a arquitetura, organização, padrões e regras **já existentes** neste repositório.
Ele **não** define uma nova arquitetura — apenas registra e reforça como o projeto está estruturado hoje,
para manter consistência entre:

- Desenvolvedores da equipe
- Agentes de IA
- Novos contribuidores

> Contexto importante: `jff-csharp-tools` é uma **biblioteca** (conjunto de pacotes NuGet), não uma aplicação.
> Logo, o foco aqui é a organização interna do código e os fluxos típicos quando um projeto consumidor usa a biblioteca.

---

## Visão Geral

O repositório publica múltiplos pacotes NuGet:

- Um **núcleo compartilhado** (`jff-csharp-tools`, `netstandard2.1`) contendo entidades base, modelos, filtros e extensões.
- Pacotes **versionados por runtime** (`jff-csharp-tools-6`, `-8`, `-9`, `-10`, e variações) que adicionam integração com:
  - Entity Framework Core (implementação de repositório)
  - ASP.NET Core (controllers, filtros, endpoints/minimal APIs)

Essa divisão permite que aplicações consumidoras escolham o pacote compatível com a versão do .NET usada no projeto.

Documento relacionado: `docs/adr/ADR-001.md`.

---

## Stack (inferida do repositório)

- Linguagem: C#
- Plataformas alvo: `netstandard2.1` (core), `net6.0`, `net8.0`, `net9.0`, `net10.0`
- ORM: Entity Framework Core (nos pacotes versionados)
- HTTP: ASP.NET Core MVC e, nas versões mais recentes, helpers para Minimal APIs
- Autenticação/Identidade: JWT Bearer (extração de usuário corrente a partir de claims)
- Utilitários: CsvHelper, System.Text.Json, extensões de domínio (LINQ/string/datetime/enum)

---

## Organização do Repositório

Estrutura de alto nível (principais diretórios):

```txt
docs/
  adr/                 Decisões arquiteturais (ADRs)
  architecture/        Diagramas PlantUML (C4) e este documento
  flows/               Fluxos de runtime (ex.: autenticação, CRUD)
  database/            Documentação de schema conceitual (entidades base)

jff-csharp-tools/      Projeto core (netstandard2.1): Domain + Application (+ utilitários)
jff-csharp-tools-6/    Pacote .NET 6: Apresentation + Infra
jff-csharp-tools-8/    Pacote .NET 8: Apresentation + Infra
jff-csharp-tools-9/    Pacote .NET 9: Apresentation + Infra (+ endpoints/minimal APIs)
jff-csharp-tools-10/   Pacote .NET 10: Apresentation + Infra (+ endpoints/minimal APIs)
jff-csharp-tools-lib-9/ Pacote .NET 9 (variante "lib"): Infra (sem MVC)
utils/                Scripts auxiliares (release/tag)
```

> Observação: o projeto usa a grafia `Apresentation/` (em vez de `Presentation/`). Este documento preserva a nomenclatura existente.

---

## Projetos / Pacotes (por responsabilidade)

Resumo do que cada projeto entrega:

- `jff-csharp-tools/` (core, `netstandard2.1`)
  - **Domain/**: entidades base, filtros, extensões, enums, exceções, constantes e interfaces (contratos).
  - **Application/**: serviços (regras de aplicação), DTOs e envelopes de resultado (`Result<T>`).
  - **Apresentation/** (no core): utilitários/exceções auxiliares usados pela biblioteca.
- `jff-csharp-tools-6/`, `jff-csharp-tools-8/`, `jff-csharp-tools-9/`, `jff-csharp-tools-10/` (pacotes runtime)
  - **Infra/**: implementações concretas (ex.: `DefaultRepository<T>` com EF Core).
  - **Apresentation/**: integração HTTP (controllers, filtros, extensions, providers e endpoints).
- `jff-csharp-tools-lib-9/`
  - **Infra/**: variante sem MVC, focada em componentes de infraestrutura para .NET 9.

Todos os pacotes runtime referenciam o core via `ProjectReference` para reutilizar entidades, contratos e serviços.

---

## Camadas e Dependências (padrão predominante)

A organização se aproxima de um estilo **em camadas** (inspirado em Clean Architecture), com separação clara de responsabilidades:

1. **Domain (Core)** — regras e modelos fundamentais
   - Entidades base (`DefaultEntity`, `DefaultGuidEntity`, `LogSerilog*Entity`)
   - Filtros (`DefaultFilter`, `PredicateBuilderFilter`)
   - Extensões (ex.: `LinqExtensions`, `StringExtension`, `DateTimeExtension`)
   - Contratos (`IDefaultRepository`, etc.)
2. **Application (Core)** — casos de uso genéricos e regras de aplicação
   - Serviços (`DefaultService<T>`, `DefaultGuidService<T>`) orquestram CRUD e validações (ex.: filtro por usuário)
   - Envelope de retorno `Result<T>` padroniza respostas/erros/metadados
3. **Infra (Runtime packages)** — implementação de persistência e integrações
   - `DefaultRepository<T>` implementa `IDefaultRepository` usando EF Core e `DbContext`
4. **Apresentation (Runtime packages)** — superfície HTTP e pipeline
   - Controllers base (ex.: `DefaultController`, `DefaultCRUDController<,>`)
   - Filtros (ex.: `ExceptionFilter`, `TokenEnumFilter`, `UnitOfWorkFilter`)
   - Helpers de Minimal API (ex.: `CrudEndpoints`) nas versões mais novas

Regra prática de dependência (o que o código faz hoje):

- `Apresentation` depende de `Application` e `Domain`
- `Application` depende de `Domain`
- `Infra` depende de `Domain` (e do runtime/EF Core)
- `Domain` evita depender de `Apresentation` e de `Infra`

---

## Fluxo de Dados (runtime)

O fluxo típico (MVC) quando um projeto consumidor usa a biblioteca é:

1. **Request HTTP** chega ao ASP.NET Core
2. (Opcional) **Filtros** validam/extraiem token (`TokenEnumFilter`) e tratam exceções (`ExceptionFilter`)
3. **Controller base** obtém o usuário corrente via JWT (`CurrentIdUser_FromBearerToken`)
4. **Serviço** executa operação CRUD via contrato (`IDefaultService`)
5. **Repositório (EF Core)** acessa o banco via `DbContext` (`IDefaultRepository`)
6. A resposta retorna embrulhada em `Result<T>` (e convertida em HTTP result)

Regras de comportamento relevantes e recorrentes:

- **Auditoria**: `CreatedAt`/`UpdatedAt` são preenchidos pelo serviço no create/update.
- **Isolamento por usuário**: por padrão, as operações filtram por `CreatorUserId == IdUser` quando `filterCurrentUser = true`.
- **Paginação**: `PaginationResult<TEntity>` carrega parâmetros e resultados paginados.

Detalhamento e diagramas de fluxo: `docs/flows/main-flows.md`.

---

## Convenções e Padrões Recorrentes

- **Base entities**: entidades do consumidor normalmente herdam de `DefaultEntity<T>` (int) ou `DefaultGuidEntity<T>` (Guid).
- **Contratos**: interfaces como `IDefaultRepository` e `IDefaultService` ficam no core, para serem reutilizadas em todos os pacotes.
- **Implementações**:
  - `DefaultService<T>` (core) implementa o contrato de serviço e depende de `IDefaultRepository`.
  - `DefaultRepository<T>` (pacotes runtime) implementa `IDefaultRepository` sobre EF Core.
- **HTTP**:
  - MVC: controllers base em `Apresentation/Controllers/`.
  - Minimal APIs (quando disponível): endpoints em `Apresentation/Endpoints/` + extensões para retornar `Result<T>`.
- **Namespaces**:
  - Core: `JffCsharpTools.*`
  - Pacotes versionados: `JffCsharpTools6.*`, `JffCsharpTools8.*`, `JffCsharpTools9.*`, `JffCsharpTools10.*`

---

## Diagramas (PlantUML / C4)

Diagramas existentes em `docs/architecture/`:

- `docs/architecture/context.puml` — contexto do sistema
- `docs/architecture/containers.puml` — containers/pacotes
- `docs/architecture/components.puml` — componentes principais

Como visualizar:

- Abra os `.puml` com uma extensão de PlantUML (ex.: VS Code PlantUML) e renderize localmente.

---

## Documentos Relacionados

- Decisão arquitetural: `docs/adr/ADR-001.md`
- Fluxos principais (runtime): `docs/flows/main-flows.md`
- Schema conceitual (entidades base): `docs/database/schema.md`

---

## Checklist para Mudanças (IA e contribuições)

Antes de criar/modificar código:

- A estrutura segue o padrão existente de camadas e pastas?
- Existe implementação semelhante que pode ser reutilizada?
- O fluxo atual (controller/endpoints → service → repository → EF Core) foi preservado?
- Foi evitada duplicação de lógica e a criação de novos paradigmas sem necessidade?
- Dependências entre camadas permaneceram coerentes (Domain/Application não dependem de Infra/Apresentation)?

