using System.IO;

namespace Miru.Tests.Core;

public class MiruPathTest
{
    [Test]
    public void Can_combine_two_paths()
    {
        (MiruPath.CurrentPath / "src" / "Shoppers.Tests")
            .ToString()
            .ShouldBe(Path.Combine(MiruPath.CurrentPath, "src", "Shoppers.Tests"));
    }
        
    [Test]
    public void Can_combine_miru_path_with_object()
    {
        (MiruPath.CurrentPath / 1000 / 2000)
            .ToString()
            .ShouldBe(Path.Combine(MiruPath.CurrentPath, "1000", "2000"));
    }
                
    [Test]
    public void Can_add_a_separator_in_the_end()
    {
        (MiruPath.CurrentPath / "src" / "Shoppers.Tests" / ".")
            .ToString()
            .ShouldBe(Path.Combine(MiruPath.CurrentPath, "src", "Shoppers.Tests") + Path.DirectorySeparatorChar);
    }
        
    [Test]
    public void Can_create_combining_dir()
    {
        (new MiruPath("src") / "Shoppers.Tests" / "bin")
            .ToString()
            .ShouldBe(Path.Combine("src", "Shoppers.Tests", "bin"));
    }
    
    [Test]
    public void If_first_path_is_slash_when_combining_should_ignore_first_path()
    {
        (new MiruPath("/") / "bin")
            .ToString()
            .ShouldBe("/bin");
    }
    
    // [Test]
    // public void If_second_path_is_slash_when_combining_should_ignore_second_path()
    // {
    //     (new MiruPath("/bin") / "/")
    //         .ToString()
    //         .ShouldBe("/bin");
    // }
}