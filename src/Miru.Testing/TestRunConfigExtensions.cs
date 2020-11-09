using System;

namespace Miru.Testing
{
    public static class TestRunConfigExtensions
    {
        public static void BeforeCase(this TestRunConfig run, Action<TestFixture> action)
        {
            run.BeforeEach((test, _) =>
            {
                if (test.Implements<IManyCasesPerTest>())
                    action(_);
            });
            
            run.BeforeAll((test, _) =>
            {
                if (test.Implements<IOneCasePerTest>())
                    action(_);
            });
        }
        
        public static void BeforeCase<TDecoration>(this TestRunConfig run, Action<TestFixture> action)
        {
            run.BeforeEach((test, _) =>
            {
                if (test.Implements<IManyCasesPerTest>() && test.Implements<TDecoration>())
                    action(_);
            });
            
            run.BeforeAll((test, _) =>
            {
                if (test.Implements<IOneCasePerTest>() && test.Implements<TDecoration>())
                    action(_);
            });
        }
    }
}