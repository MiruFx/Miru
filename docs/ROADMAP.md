# Miru Roadmap

This is a Roadmap plan but not a commitment. It can change in the future.

## Phases

2021/12: 1.0 - Ready for production and .NET 6
2022/12: 2.0 - Improve 1.0 from feedbacks

## Versions


### 0.5 - Feb

~~Samples~~
  * ~~Reduce numbers of samples to maintain: only Supportreon~~

~~Userfy~~
  * ~~Register~~  
  * ~~Login~~
  * ~~Forgot password~~
    * ~~Send email~~
  * ~~AuthorizationRules~~
  * ~~ICanBeAdmin~~
  * ~~Change password~~

~~AppSettings.yml~~
 * ~~stop using config/Config.{Env}.yml~~
 * ~~use appsetings.yml, appsetings.{Env}.yml~~

### 0.6 - Mar

Command Line:
miru --version or miru -v
outside solution:
mirucli's version
inside
miru's version in the solution
mirucli's version

- Consolable:
    - Replace Oakton for Microsoft Commandline or other
    - Support execute for all environment '-e all'
    - IConsolable could be decorated with IMultipleEnvironment allowing run -e all
    - ConfigYmlFinder can search all config.$env.yml

- Queueing:
    - Different pipeline for _mediator.Send(job)
    - Schedule jobs by time
    - Automated tests for Queueing features
    - Consider using https://github.com/xavierjefferson/Hangfire.FluentNHibernateStorage?
    - Mediator Scoped
    - IsAdmin for Dashboard

- Seed Data
    - miru make:seed UsersSeed
    - miru seed:run? or miru db:seed?

- Storage: Local
    - Upload images/files - Supportreon: project's image add/edit
        - Validation
        - Save in file
        - Save in db

- Urls:
    - Import FubuMvc 'object to url'

### 0.7 - Apr - EfCore PostgreSql, FrontEnd

- Userfy
  Register from 3rd party
  Login from 3rd party
  Register confirmation
  Send email

- Flash messages
    - AccountRegister can add success message
    - Integration with Turbo

### 0.8 - Mai -

### 0.9 - Jun -  - C# 9, Build script, Http API

- Http Api
    - Authentication, Token
    - Respond to xml, json, etc
    - ApiTest

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

### 0.12 - Sep - Ajax, Turbo

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

- Caching
- Gzip assets

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
