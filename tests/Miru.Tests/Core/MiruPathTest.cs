using System.IO;

namespace Miru.Tests.Core;

public class MiruPathTest
{
    [Test]
    public void Can_combine_two_paths()
    {
        (A.Path / "src" / "Shoppers.Tests")
            .ToString()
            .ShouldBe(Path.Combine("src", "Shoppers.Tests"));
    }
        
    [Test]
    public void Can_combine_miru_path_with_object()
    {
        (A.Path / 1000 / 2000)
            .ToString()
            .ShouldBe(Path.Combine("1000", "2000"));
    }
                
    [Test]
    public void Can_add_a_separator_in_the_end()
    {
        (A.Path / "src" / "Shoppers.Tests" / ".")
            .ToString()
            .ShouldBe(Path.Combine("src", "Shoppers.Tests"));
    }
        
    [Test]
    public void Can_create_combining_dir()
    {
        (new MiruPath("src") / "Shoppers.Tests" / "bin")
            .ToString()
            .ShouldBe(Path.Combine("src", "Shoppers.Tests", "bin"));
    }
    
    [Test]
    public void Should_combine_static_a_path_with_other_path()
    {
        (A.Path / "assets" / "favicon.png")
            .ToString()
            .ShouldBe(Path.Combine("assets", "favicon.png"));
    }
    
    [Test]
    public void If_first_path_is_slash_when_combining_should_ignore_first_path()
    {
        (new MiruPath("/") / "bin")
            .ToString()
            .ShouldBe("bin");
    }
    
    [Test]
    public void If_alt_separator_then_all_combination_should_use_alt_separator()
    {
        (new MiruPath("src", altSeparator: true) / "Shoppers.Tests" / "bin")
            .ToString()
            .ShouldBe("src/Shoppers.Tests/bin");
    }
}