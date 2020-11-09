using System.Collections.Generic;

namespace Miru.Testing
{
    public class TestRunner
    {
        public int RunAssemblyOfType<TType>(string[] args)
        {
            // https://github.com/nunit/docs/wiki/Test-Selection-Language
            
            // cat == Data
            // test =~ /TestCaseAttributeTest/
            // method == SomeMethodName
            // cat != Slow
            // Priority == High
            // namespace == My.Name.Space
            //     
            //
            // == to test for equality - a single equal sign (=) may be used as well and has the same meaning
            //     != to test for inequality
            //     =~ to match a regular expression
            // !~ to not match a regular expression
            //     
            
            var testArgs = new List<string>
            {
                "--noresult",
                "--labels=All",
                "--trace=Off",
                "--noheader"
            };

            if (args.Length > 0)
            {
                testArgs.Add("--where");
                testArgs.Add($"class=~/{args[0]}/");
            }

            var executeArgs = testArgs.ToArray();
            
            return new MiruTestRunner(typeof(TType).Assembly).Execute(executeArgs);
        }
    }
}