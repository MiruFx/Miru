<!--
Storage
    intro
    directories

Storage Drivers
    local disk
        /storage/app

Storage `Spaces`?
    better name for spaces
    App
    Assets
    Temp()
    Db

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

Name Conventions
    fullPath -> a file/directory complete path in the storage (e.g. C:\storage\avatar-12.png)
    
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

## High Level Paths

Miru's Storage's design provides a way to use **High Level Paths**
in your application. Instead of using `_storage.App / "invoices" / $"invoice-{invoice.Id}.pdf"`
you can use `_storage.App.InvoicePdf(invoice)`.

Create a class `MiruPathExtensions` and add methods for you Application's files/directories:

```csharp
public static class MiruPathExtensions
{
    public static MiruPath InvoicePdf(this MiruPath path, Invoice invoice) =>
        path / "invoices" / $"invoice-{invoice.Id}.pdf";
}
```

In your `Features' Handlers` or other services, you use your
**High Level Paths** through `IStorage`'s App property:

```csharp
public class Handler : IRequestHandler<Query, Result>
{
    private readonly AppDbContext _db;
    private readonly IStorage _storage;

    public Handler(AppDbContext db, IStorage storage)
    {
        _db = db;
        _storage = storage;
    }
        
    public async Task<Command> Handle(Query request, CancellationToken ct)
    {
        var invoice = await _db.Invoices.ByIdAsync(request.InvoiceId, ct);
        
        var invoicePath = _storage.App.InvoicePdf(invoice);
        
        return new Result
        {
            InvoicePath = invoicePath
        };
    }
}
```

Since the **High Level Paths** are extensions of `MiruPath` class, 
you can use your custom paths in other storage's directories:

```csharp
var tempInvoice = _storage.Temp().InvoicePdf(invoice);

var customDirInvoice = (_storage.StorageDir / "custom").InvoicePdf(invoice);
```

## Consolables

### Storage Link

Since all Application's assets are saved into `/storage/assets` and the
Web Application's static assets are mapped in `/src/{App}/wwwroot`, we can
create a symlink between `/storage/assets/public` and `/src/{App}/wwwroot/public` using `storage.link` command:

```shell
miru storage.link
```