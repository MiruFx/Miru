using System;

namespace Miru.Testing
{
    public class TestRunConfig
    {
        internal readonly TestSetupActions ActionsBeforeSuite = new TestSetupActions();

        internal readonly TestSetupActions ActionsBeforeAll = new TestSetupActions();

        internal readonly TestSetupActions ActionsBeforeEach = new TestSetupActions();
        
        internal readonly TestSetupActions ActionsAfterEach = new TestSetupActions();
        
        internal readonly TestSetupActions ActionsAfterAll = new TestSetupActions();
        
        internal readonly TestSetupActions ActionsAfterSuite = new TestSetupActions();

        public void BeforeSuite(Action<TestFixture> action)
        {
            ActionsBeforeSuite.Add(typeof(object), (type, fixture) => action(fixture));
        }
        
        public void BeforeSuite<TTestType>(Action<TestFixture> action)
        {
            ActionsBeforeSuite.Add(typeof(TTestType), (type, fixture) => action(fixture));
        }
        
        public void BeforeAll(Action<TestFixture> action)
        {
            ActionsBeforeAll.Add(typeof(object), (type, fixture) => action(fixture));
        }
        
        public void BeforeAll<TTestType>(Action<TestFixture> action)
        {
            ActionsBeforeAll.Add(typeof(TTestType), (type, fixture) => action(fixture));
        }

        public void BeforeAll(Action<Type, TestFixture> action)
        {
            ActionsBeforeAll.Add(typeof(object), action);
        }

        public void BeforeEach(Action<TestFixture> action)
        {
            ActionsBeforeEach.Add(typeof(object), (type, fixture) => action(fixture));
        }
        
        public void BeforeEach(Action<Type, TestFixture> action)
        {
            ActionsBeforeEach.Add(typeof(object), action);
        }
        
        public void BeforeEach<TTestType>(Action<TestFixture> action)
        {
            ActionsBeforeEach.Add(typeof(TTestType), (type, fixture) => action(fixture));
        }
        
        public void AfterEach(Action<TestFixture> action)
        {
            ActionsAfterEach.Add(typeof(object), (type, fixture) => action(fixture));
        }
        
        public void AfterEach(Action<Type, TestFixture> action) =>
            ActionsAfterEach.Add(typeof(object), action);
        
        public void AfterEach<TTestType>(Action<TestFixture> action)
        {
            ActionsAfterEach.Add(typeof(TTestType), (type, fixture) => action(fixture));
        }

        public void AfterAll(Action<TestFixture> action)
        {
            ActionsAfterAll.Add(typeof(object), (type, fixture) => action(fixture));
        }
        
        public void AfterAll(Action<Type, TestFixture> action) =>
            ActionsAfterAll.Add(typeof(object), action);
        
        public void AfterAll<TTestType>(Action<TestFixture> action)
        {
            ActionsAfterAll.Add(typeof(TTestType), (type, fixture) => action(fixture));
        }
        
        public void AfterSuite(Action<TestFixture> action)
        {
            ActionsAfterSuite.Add(typeof(object), (type, fixture) => action(fixture));
        }
    }
}