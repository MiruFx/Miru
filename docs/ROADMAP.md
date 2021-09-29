# Miru Roadmap

This Roadmap is a plan, but not a commitment. It can change in the future.

## Phases

2021/12: 1.0 - Ready for production and .NET 6
2022/12: 2.0 - Performance and more facilities
2023/12: 3.0 - Publish and advertise, performance

## Versions

### 0.7 - Mai - Storage, Internationalization, Localization

- Queueing:
  - IsAdmin for Dashboard
  
- Internationalization
  - Dates
  - Money
  - Decimal 
  
- Localization
  - Calendar (months' names)
  - FluentValidation errors
    - FluentValidation read 'Display' attribute
  - Miru default errors

- Storage: Local
  - Upload images/files - Supportreon: project's image add/edit
    - Validation
    - Save in file
    - Save in db
 
- Seed Data
  - miru make:seed UsersSeed
  - miru seed:run? or miru db:seed?
  
### 0.8 - Mai -

- Flash messages
  - AccountRegister can add success message
  - Integration with Turbo

- Consolable:
  - Replace Oakton for Microsoft Commandline or other
  - Support execute for all environment '-e all'
  - IConsolable could be decorated with IMultipleEnvironment allowing run -e all
  - ConfigYmlFinder can search all config.$env.yml
  
### 0.9 - Jun - C# 9, Build script

- Use C# 9:
    - Records
    - Nulls
    - Fabrication and 'with'

- Add Scripts\Build for new solution

### 0.10 - Jul - Testing

- New Testings:
    - HtmlTest
        - A way of test app's htmlconfig: HtmlTest
    - HttpTest
    - AuthorizationTest

### 0.11 - Aug - UI

- Localization and Internationalization
    - Dates and Timezones

- SystemClock? Utc

- Tables:
    - Sort
    - Filter multiselect
    - Data-remote

- Flash messages

### 0.12 - Sep - Queues & Turbo

- Queueing:
  - Support Hangfire to MySql
    - Consider using https://github.com/xavierjefferson/Hangfire.FluentNHibernateStorage?
  - Different pipeline for _mediator.Send(job)
  - Automated tests for Queueing features
  - Mediator Scoped
  
- Turbo:
    - Support turbo-stream with websockets

### 0.13 - Oct - Documentation & Onboard

Corpo.Skeleton
- Add default Readme.md for new solutions
- Export stubs
- Fix all makers
- Add comments through the source code
- Better HomeIndex with links to Miru documentation

Docs:
mirufx.github.com/docs

### 0.14 - Nov - Performance

- Urls:
  - Import FubuMvc 'object to url'
  
### 0.15 - Dec - 1.0 alpha/beta

Default configurations (Log, Database, Criptography)
Development
Test
Staging
Production

Use real automated tests to check if support to different combination of apps are working.
- SqlServer with Hangfire
- Postgre with Hangfire

PageTests
- Run tests against published compiled app

### 1.0 - 31/December/2021 - Ready for Production

- Update to .NET 6

- Deploying & Production
    - Install docker
    - Container with .net 5
    - Publish binaries: dotnet publish
    - Run page tests over published
    - Running from compiled
    - Minimized frontend resources Production

### 2.0 - 31/December/2022 - Performance

- Caching
  - Entity Framework Second Level caching
  - Russian-doll caching
    - (EF changes can expire UI cachings)
- Gzip assets
- Run tests in parallel
- Fabrication performance
- Url generation performance
  - Refactor the whole code
  - Benchmark and improve performance
- Use Result instead of Exceptions for Validation, Domain, NotFound exception