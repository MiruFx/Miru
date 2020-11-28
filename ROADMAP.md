# Miru Roadmap

The roadmap can change in the future.

### 0.4 - Async suffix, sample with SqlServer, Queueing, miru new

- Invert methods suffix adding Async and removing Sync
- SelfImprov:
    - Use SqlServer instead of Sqlite
- Command Line:
    - 'miru new' instead of 'mirucli new'
    - 'miru --version' instead of 'mirucli --version'
- Makers
    - Add maker for Email: "miru make:email"
    - Add maker for Job: "miru make:job"
- Queueing:  
    - Support Hangfire.SqlServer

    
### 0.5 - Queueing, Storage, Consolable, Better Url To Model

- EfCore:
    - Split SqlServer and Sqlite
    
- Queueing:      
    - Different pipeline for _mediator.Send(job)
    - Schedule jobs by time
    - Support Hangfire.SqlServer
    - Automated tests for Queueing features
    - Consider using https://github.com/xavierjefferson/Hangfire.FluentNHibernateStorage?
        
- Storage: Local
    - Upload images/files - Supportreon: project's image add/edit
        - Validation
        - Save in file
        - Save in db
    
- Consolable:
    - Replace Oakton for Microsoft Commandline or other
    - Support execute for all environment '-e all'
    - IConsolable could be decorated with IMultipleEnvironment allowing run -e all
    - ConfigYmlFinder can search all config.$env.yml

- Urls:
    - Import FubuMvc 'object to url'

### 0.6 - Identity, Seed Data
  
- Identity:
    - Use whatever is possible from Identity
    - Extract Miru.Userfy?
        - miru @app dotnet add package Miru.Userfy
        - miru userfy:install 
                  
- Seed Data
    - miru make:seed UsersSeed
    - miru seed:run? or miru db:seed?
        
### 0.7 - EfCore PostgreSql, FrontEnd

- EfCore PostgreSql:
    - Make Supportreon use EfCore with PostgreSql
   
- EfCore Split:
    - Miru.EfCore.Sqlite
    - Miru.EfCore.SqlServer
    - Miru.EfCore.PostgreSql
    
- FrontEnd
    - Update to Boostrap 5
        - Remove jQuery
        - Remove expose-loader
    - Review package.json dependencies dependencies

### 0.8


### 0.9 - C# 9, Build script

- Use C# 9:
    - Records
    - Nulls
    - Fabrication and 'with'
        
- Add Scripts\Build for new solution
    
### 0.10 - Testing

- New Testings:
    - HtmlTest
        - A way of test app's htmlconfig: HtmlTest
    - HttpTest
    - AuthorizationTest
        
### 0.11 - UI

- Localization and Internationalization
    - Dates and Timezones
    
- Tables:
    - Sort
    - Filter multiselect
    - Data-remote
        
- Flash messages

### 0.12 - Ajax, Turbolinks

- Turbolinks:
    - Full Turbolinks: https://khalidabuhakmeh.com/use-aspnet-with-turbolinks-5
        
### 0.13
### 0.14 - Performance

- Caching
- Gzip assets
    
### 0.15 - Documentation & Onboard

Default configurations (Log, Database, Criptography)
    Development
    Test
    Staging
    Production
    
### 1.0 - December/2021 - Ready for Production

- Update to .NET 6
    
- Deploying & Production
    - Install docker
    - Container with .net 5
    - Publish binaries: dotnet publish
    - Run page tests over published        
    - Running from compiled
    - Minimized frontend resources Production
    
### Future

- Support other ORMs rather than EfCore
    - Marten
    - NHibernate?
