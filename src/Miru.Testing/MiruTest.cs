using System;
using NUnit.Framework;
using Serilog;

namespace Miru.Testing;

public abstract class MiruTest
{
    private static readonly Lazy<ILogger> logger = new(Serilog.Log.ForContext<MiruTest>);

    public static ILogger Log => logger.Value;

    private readonly TestConfigRunner _testConfigRunner;

    protected readonly TestFixture _;

    public MiruTest()
    {
        var app = TestMiruHost.StartOrGetApp(GetType().Assembly);

        _ = app.Get<TestFixture>();
            
        _testConfigRunner = app.Get<TestConfigRunner>();
    }

    [OneTimeSetUp]
    public void BaseOneTimeSetup()
    {
        Log.Measure($"RunBeforeAll {GetType()}", () => 
            _testConfigRunner.RunBeforeAll(GetType()));
    }
        
    [SetUp]
    public void BaseTestSetup()
    {
        Log.Measure($"RunBeforeEach {GetType()}", () => 
            _testConfigRunner.RunBeforeEach(GetType()));
    }

    [TearDown]
    public void BaseTestTeardown()
    {
        Log.Measure($"RunAfterEach {GetType()}", () => 
            _testConfigRunner.RunAfterEach(GetType()));
    }
        
    [OneTimeTearDown]
    public void BaseTestOneTimeTeardown()
    {
        Log.Measure($"RunAfterAll {GetType()}", () => 
            _testConfigRunner.RunAfterAll(GetType()));
    }
}