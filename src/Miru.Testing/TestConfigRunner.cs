using System;
using Baseline;
using Microsoft.Extensions.Logging;

namespace Miru.Testing
{
    public class TestConfigRunner
    {
        private readonly TestRunConfig _testRunConfig;
        private readonly TestFixture _testFixture;
        private readonly ILogger<TestConfigRunner> _logger;

        public TestConfigRunner(TestRunConfig testRunConfig, TestFixture testFixture, ILogger<TestConfigRunner> logger)
        {
            _testRunConfig = testRunConfig;
            _testFixture = testFixture;
            _logger = logger;
        }
        
        public void RunBeforeSuite()
        {
            _logger.LogDebug($"Running BeforeSuite");
            ExecuteActionsByType(_testRunConfig.ActionsBeforeSuite, typeof(object));
            _logger.LogDebug($"Finished BeforeSuite");
        }

        public void RunBeforeAll(Type currentTestType)
        {
            _logger.LogDebug($"Running BeforeAll for {currentTestType.FullName}");
            ExecuteActionsByType(_testRunConfig.ActionsBeforeAll, currentTestType);
            _logger.LogDebug($"Finished BeforeAll for {currentTestType.FullName}");
        }
        
        public void RunBeforeEach(Type currentTestType)
        {
            _logger.LogDebug($"Running BeforeEach for {currentTestType.FullName}");
            ExecuteActionsByType(_testRunConfig.ActionsBeforeEach, currentTestType);
            _logger.LogDebug($"Finished BeforeEach for {currentTestType.FullName}");
        }
        
        public void RunAfterEach(Type currentTestType)
        {
            _logger.LogDebug($"Running AfterEach for {currentTestType.FullName}");
            ExecuteActionsByType(_testRunConfig.ActionsAfterEach, currentTestType);
            _logger.LogDebug($"Finished AfterEach for {currentTestType.FullName}");
        }
        
        public void RunAfterAll(Type currentTestType)
        {
            _logger.LogDebug($"Running AfterAll for {currentTestType.FullName}");
            ExecuteActionsByType(_testRunConfig.ActionsAfterAll, currentTestType);
            _logger.LogDebug($"Finished AfterAll for {currentTestType.FullName}");
        }
        
        public void RunAfterSuite()
        {
            _logger.LogDebug($"Running AfterSuite");
            ExecuteActionsByType(_testRunConfig.ActionsAfterSuite, typeof(object));
            _logger.LogDebug($"Finished AfterSuite");
        }

        private void ExecuteActionsByType(TestSetupActions actions, Type currentRunningTestType)
        {
            foreach (var action in actions.All())
            {
                if (currentRunningTestType.ImplementsInterfaceTemplate(action.Key))
                {
                    _logger.LogDebug($"Running {action.Key.FullName} action");
                    action.Value(currentRunningTestType, _testFixture);
                }

                if (action.Key.IsAssignableFrom(currentRunningTestType))
                {
                    action.Value(currentRunningTestType, _testFixture);
                }
            }
        }
    }
}