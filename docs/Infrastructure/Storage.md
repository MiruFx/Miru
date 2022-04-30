<!--
Storage
    intro
    directories

Storage Drivers
    local disk
        /storage/app

Application's Assets
    local disk
        /storage/assets

MiruPath
    path = file or directory
    
Configuration
    AddStorage

Using Storages
    IStorage
    Retrieving Files
    Saving Files
    Paths
    High level paths
        storage.App.Invoice(invoice)

Many Storages
    IFedexStorage

-->

[[toc]]

# Storage

Miru comes with a concept called Storage.
<!--
Applications need to store and retrive files.
Some files are required for an Application to run properly, for example:
images, javascripts, css files, json files, and etc. 
-->

## Storage Directory Structure

| Relative Path            | Description                                                                                  | git    |
|--------------------------|----------------------------------------------------------------------------------------------|--------|
| `/storage`               | Storage root                                                                                 | commit |
| `/storage/assets`        | Files required for the Application to run properly                                           | commit |
| `/storage/assets/public` | Files that will be exposed on the webserver (wwwroot/public)                                 | commit |
| `/storage/app`           | Directory where the Application can save user's files                                        | ignore |
| `/storage/db`            | Directory where some database can be stored                                                  | ignore |
| `/storage/temp`          | Temporary files that can be deleted from time to time                                        | ignore |
| `/storage/logs`          | Where the Application save logs                                                              | ignore |
| `/storage/tests`         | Replicates the same structure of /storage but for Testing. It is erased before a test is run | ignore |

## Consolables

### Storage Link

Since all Application's assets are saved into `/storage/assets` and the
Web Application's static assets are mapped in `/src/{App}/wwwroot`, we can
create a symlink between `/storage/assets/public` and `/src/{App}/wwwroot/public` using `storage.link` command:

```shell
miru storage.link
```