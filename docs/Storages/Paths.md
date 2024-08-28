# Storage Paths

`Miru Storage` provides helpers to deal with Paths handling.

## MiruPath

`MiruPath` is a special `struct` type that represents a `Path` to a file or directory. It is a better alternative than storing paths as string.

```csharp
MiruPath productImagePath = "/storage/app/products/product-1.png";
~~~~
productImagePath.Dir(); // /storage/app/products

MiruPath archiveImagesDir = "/storage/app/archive/products";
MiruPath archiveProductImagePath = archiveImagesDir / productImagePath.FileName();

archiveProductImagePath; // /storage/app/archive/products/product-1.png
```

## A.Path

## Storage.Path

## AppPaths

## Scoped Path Extensions

If your application needs to import or export files from or to third parties, you can create `Path Extensions` 
to represent the paths that these files are stored.
                     
