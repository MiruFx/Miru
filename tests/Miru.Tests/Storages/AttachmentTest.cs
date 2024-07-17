using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;
using Miru.Foundation.Logging;
using Miru.Storages;
using Miru.Tests.Queuing;

namespace Miru.Tests.Storages;

public class AttachmentTest : MiruCoreTest
{
    private IAppStorage _storage;

    public override IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services.AddStorages();
    }

    [SetUp]
    public void Setup()
    {
        _storage = _.AppStorage();
    }
    
    [Test]
    public void Should_create_an_attachment_from_file_path()
    {
        // arrange
        var file = (_storage.Path / "file.txt").MakeFake(letters: 8);
        using var stream = _storage.GetAsync(file);
            
        // act
        var attachment = new Attachment(_storage, file);
        
        // assert
        attachment.Key.ShouldBe("file.txt");
        attachment.FileName.ShouldBe("file.txt");
        attachment.ByteSize.ShouldBe(8);
        attachment.ContentType.ShouldBe("text/plain");
        attachment.Checksum.ShouldBe(file.GetMd5());
    }

    [Test]
    [Ignore("WIP")]
    public void Should_create_an_attachment_from_form_file()
    {
        // arrange
        // var file = (_storage.Path / "file.txt").MakeFake();
        // using var stream = _storage.GetAsync(file);
        //     
        // // act
        // var attachment = new Attachment(file, stream,)
        // // assert
    }

    [Test]
    [Ignore("WIP")]
    public void Should_update_current_attachment_metadata_with_other_attachment()
    {
        // arrange
        
        // act
        
        // assert
    }
}