using System;
using Baseline;
using Microsoft.Extensions.Logging;

namespace Miru.Testing;

public class TestConfigRunner
{
    private readonly TestRunConfig _testRunConfig;
    private readonly TestFixture _testFixture;

    public TestConfigRunner(
        TestRunConfig testRunConfig, 
        TestFixture testFixture)
    {
        _testRunConfig = testRunConfig;
        _testFixture = testFixture;
    }
        
    public void RunBeforeSuite()
    {
        MiruTest.Log.Measure("RunBeforeSuite", () => 
            ExecuteActionsByType(_testRunConfig.ActionsBeforeSuite, typeof(object)));
    }

    public void RunBeforeAll(Type currentTestType)
    {
        MiruTest.Log.Measure("RunBeforeAll", () =>
        {
            MiruTest.Log.Debug($"Running BeforeAll for {currentTestType.FullName}");
                
            ExecuteActionsByType(_testRunConfig.ActionsBeforeAll, currentTestType);
                
            MiruTest.Log.Debug($"Finished BeforeAll for {currentTestType.FullName}");
        });
    }
        
    public void RunBeforeEach(Type currentTestType)
    {
        MiruTest.Log.Measure("RunBeforeEach", () =>
        {
            MiruTest.Log.Debug($"Running BeforeEach for {currentTestType.FullName}");
                
            ExecuteActionsByType(_testRunConfig.ActionsBeforeEach, currentTestType);
                
            MiruTest.Log.Debug($"Finished BeforeEach for {currentTestType.FullName}");
        });
    }
        
    public void RunAfterEach(Type currentTestType)
    {
        MiruTest.Log.Measure("RunAfterEach", () =>
        {
            MiruTest.Log.Debug($"Running AfterEach for {currentTestType.FullName}");
                
            ExecuteActionsByType(_testRunConfig.ActionsAfterEach, currentTestType);
                
            MiruTest.Log.Debug($"Finished AfterEach for {currentTestType.FullName}");
        });
    }
        
    public void RunAfterAll(Type currentTestType)
    {
        MiruTest.Log.Measure("RunAfterAll", () =>
        {
            MiruTest.Log.Debug($"Running AfterAll for {currentTestType.FullName}");
                
            ExecuteActionsByType(_testRunConfig.ActionsAfterAll, currentTestType);
                
            MiruTest.Log.Debug($"Finished AfterAll for {currentTestType.FullName}");
        });
    }
        
    public void RunAfterSuite()
    {
        MiruTest.Log.Measure("RunAfterSuite", () =>
        {
            MiruTest.Log.Debug($"Running AfterSuite");
                
            ExecuteActionsByType(_testRunConfig.ActionsAfterSuite, typeof(object));
                
            MiruTest.Log.Debug($"Finished AfterSuite");
        });
    }

    private void ExecuteActionsByType(TestSetupActions actions, Type currentRunningTestType)
    {
        foreach (var action in actions.All())
        {
            if (currentRunningTestType.ImplementsInterfaceTemplate(action.Key))
            {
                MiruTest.Log.Debug($"Running {action.Key.FullName} action");
                
                action.Value(currentRunningTestType, _testFixture);
            }

            if (action.Key.IsAssignableFrom(currentRunningTestType))
            {
                action.Value(currentRunningTestType, _testFixture);
            }
        }
    }
}