using System.IO;
using Miru.Core;
using MiruCli;
using NUnit.Framework;

namespace Miru.Tests.Cli
{
    public class EmbeddedFileTest
    {
        [SetUp]
        [TearDown]
        public void Setup()
        {
            Files.DeleteIfExists(A.TempPath("EmbeddedFileTest", "miru.cmd"));
        }
        
        [Test]
        public void Extract_embedded_to_a_file()
        {
            var embeddedFile = new EmbeddedFiles<MiruCli.Program>();

            embeddedFile.ExtractFile("miru.cmd", A.TempPath("EmbeddedFileTest", "miru.cmd"));

            File.Exists(A.TempPath("EmbeddedFileTest", "miru.cmd"));
        }
    }
}