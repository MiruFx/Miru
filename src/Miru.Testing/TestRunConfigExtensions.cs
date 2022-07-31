using System;

namespace Miru.Testing;

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
        
    public static void BeforeCaseInNamespace(this TestRunConfig run, string @namespace, Action<TestFixture> action)
    {
        run.BeforeEach((test, _) =>
        {
            if (test.Namespace.Contains($".{@namespace}") && test.Implements<IManyCasesPerTest>())
                action(_);
        });
            
        run.BeforeAll((test, _) =>
        {
            if (test.Namespace.Contains($".{@namespace}") && test.Implements<IOneCasePerTest>())
                action(_);
        });
    }
        
    public static void AfterCase(this TestRunConfig run, Action<TestFixture> action)
    {
        run.AfterEach((test, _) =>
        {
            if (test.Implements<IManyCasesPerTest>())
                action(_);
        });
            
        run.AfterAll((test, _) =>
        {
            if (test.Implements<IOneCasePerTest>())
                action(_);
        });
    }
        
    public static void AfterCase<TDecoration>(this TestRunConfig run, Action<TestFixture> action)
    {
        run.AfterEach((test, _) =>
        {
            if (test.Implements<IManyCasesPerTest>() && test.Implements<TDecoration>())
                action(_);
        });
            
        run.AfterAll((test, _) =>
        {
            if (test.Implements<IOneCasePerTest>() && test.Implements<TDecoration>())
                action(_);
        });
    }
}