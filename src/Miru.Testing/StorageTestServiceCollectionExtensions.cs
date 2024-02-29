using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bogus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Miru.Consolables;
using Miru.Core;
using Miru.Storages;
using Shouldly;

namespace Miru.Testing;

public static class StorageTestServiceCollectionExtensions
{
    public static Queue<Stream> Streams { get; set; } = new();

    public static IServiceCollection AddAppTestStorage(this IServiceCollection services)
    {
        return services.AddAppStorage<AppTestStorage>();
    }
    
    public static FormFile FormFile2(
        this ITestFixture fixture, 
        string fileName, 
        string fileContent,
        string contentType = null)
    {
        var path = fixture.AppStorage().Temp() / fileName;
        
        if (contentType is null)
            contentType = MimeTypes.MimeTypeMap.GetMimeType(path.FileExtension());

        path.Dir().EnsureDirExist();
        
        File.WriteAllText(path, fileContent);

        return fixture.FormFile(path, contentType);
    }
    
    public static FormFile FormFile(this ITestFixture fixture, MiruPath path, string contentType)
    {
        var stream = File.OpenRead(path);
            
        var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(path))
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        Streams.Enqueue(stream);
        
        return file;
    }
    
    public static FormFile MakeFormFile(
        this ITestFixture fixture, 
        string fileName)
    {
        var path = fixture.AppStorage().Temp() / fileName;
        var contentType = MimeTypes.MimeTypeMap.GetMimeType(path.FileExtension());

        path.Dir().EnsureDirExist();
        
        File.WriteAllText(path, fixture.Faker().Lorem.Sentences(3));

        return fixture.FormFile(path, contentType);
    }

    public static FormFile ToFormFile(this MiruPath path)
    {
        var stream = File.OpenRead(path);
        var contentType = MimeTypes.MimeTypeMap.GetMimeType(path.FileExtension());
            
        var file = new FormFile(stream, 0, stream.Length, null, System.IO.Path.GetFileName(path))
        {
            Headers = new HeaderDictionary(),
            ContentType = contentType
        };

        Streams.Enqueue(stream);
        
        return file;
    }

    public static ITestFixture ClearTestStorage(this ITestFixture fixture)
    {
        MiruTest.Log.Measure(nameof(ClearTestStorage), () =>
        {
            while (Streams.Count > 0)
            {
                var stream = Streams.Dequeue();
                stream.Dispose();
            }
            
            using var scope = fixture.App.WithScope();

            (scope.Get<MiruSolution>().StorageDir / "tests").DeleteDir();
        });

        return fixture;
    }
    
    public static void LinesCountShouldBe(this MiruPath fileName, int totalLines, string newLine = null)
    {
        if (newLine is null)
            newLine = Environment.NewLine;
        
        var count = File.ReadLines(fileName).Count();

        count.ShouldBe(totalLines, customMessage: $"Expecting {totalLines} but found {count} lines");
    }
    
    public static void LinesShouldBe(this MiruPath fileName, string newLine, params string[] lines)
    {
        var actualContent = File.ReadAllText(fileName);
        
        var expectationContent = new StringBuilder();

        for (int index = 0; index < lines.Length; index++)
        {
            expectationContent.Append(lines[index]);
            expectationContent.Append(newLine);
        }

        actualContent.ShouldBe(expectationContent.ToString(), customMessage: $"File {fileName}");
    }
    
    public static void ShouldContain(this MiruPath fileName, params string[] lines)
    {
        var fileContent = File.ReadAllText(fileName);

        try
        {
            foreach (var line in lines)
                fileContent.ShouldContain(line);
        }
        catch (Exception)
        {
            Console.WriteLine("==== Full file's content: ====");
            Console.WriteLine(fileContent);
            
            throw;
        }
    }
    
    public static void ShouldNotContain(this MiruPath fileName, params string[] lines)
    {
        var fileContent = File.ReadAllText(fileName);

        try
        {
            foreach (var line in lines)
                fileContent.ShouldNotContain(line);
        }
        catch (Exception)
        {
            Console.WriteLine("==== Full file's content: ====");
            Console.WriteLine(fileContent);
            
            throw;
        }
    }
    
    public static MiruPath MakeFake(this MiruPath path)
    {
        path.Dir().EnsureDirExist();
        
        File.WriteAllText(path, new Faker().Lorem.Sentence());
        
        return path;
    }
}