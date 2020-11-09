using Miru.Core;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests
{
    public class StringExtensionsTest
    {
        [Test]
        public void Equivalent_strings()
        {
            "PostgreSql".CaseCmp("postgresql").ShouldBeTrue();
            "PostgreSql".CaseCmp("PostgreSq").ShouldBeFalse();
        }

        [Test]
        public void Break_camel_case()
        {
            "FirstName".BreakCamelCase().ShouldBe("First Name");
            "Last_Name".BreakCamelCase().ShouldBe("Last Name");
        }

        [Test]
        public void Pascal_case_to_kebab_case()
        {
            string.Empty.ToKebabCase().ShouldBe(string.Empty);
            "I".ToKebabCase().ShouldBe("i");
            "IO".ToKebabCase().ShouldBe("io");
            "FileIO".ToKebabCase().ShouldBe("file-io");
            "SignalR".ToKebabCase().ShouldBe("signal-r");
            "IOStream".ToKebabCase().ShouldBe("io-stream");
            "COMObject".ToKebabCase().ShouldBe("com-object");
            "WebAPI".ToKebabCase().ShouldBe("web-api");
            "ProductNew.Command".ToKebabCase().ShouldBe("product-new-command");
        }
    }
}
