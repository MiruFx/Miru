using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Miru.Fabrication.FixtureConventions;

namespace Miru.Fabrication
{
    public class Fabricator
    {
        public readonly FabricatedSession Session;
        public readonly Fixture Fixture;
        public readonly Faker Faker;
        
        private readonly FabSupport _support;
        private readonly Dictionary<Type, object> _defaultsWith = new();

        public Fabricator(FabSupport support)
        {
            Faker = support.Faker;

            Session = support.Session;

            Fixture = support.Fixture;

            _support = support;
        }

        public T Make<T>() where T : class
        {
            var customFactory = FabFor<T>();

            T fabricated;

            if (_defaultsWith.TryGetValue(typeof(T), out var defaultWithObject))
            {
                var defaultWith = (Action<T, Faker>) defaultWithObject;
                fabricated = Fixture.Create<T>();
                defaultWith(fabricated, Faker);
            }
            else
            {
                fabricated = customFactory != null ? customFactory.Make() : Fixture.Create<T>();    
            }

            Session.Add(fabricated);
            
            return fabricated;
        }
        
        public T Make<T>(Action<T> customization) where T : class
        {
            var made = Make<T>();
            
            customization?.Invoke(made);
            
            return made;
        }
        
        public IEnumerable<T> MakeMany<T>(int count, Action<T> customization = null) where T : class
        {
            var customFactory = FabFor<T>();
            
            IEnumerable<T> allMade;

            if (_defaultsWith.TryGetValue(typeof(T), out var defaultWithObject))
            {
                var defaultWith = (Action<T, Faker>) defaultWithObject;
                
                allMade = Fixture.CreateMany<T>();
                
                foreach (var made in allMade)
                {
                    defaultWith(made, Faker);    
                }
            }
            else
            {
                allMade = customFactory != null ? customFactory.MakeMany(count) : Fixture.CreateMany<T>(count); 
            }
            
            foreach (var oneMade in allMade)
            {
                customization?.Invoke(oneMade);
            }
            
            Session.AddMany(allMade);
            
            return allMade;
        }
        
        /// <summary>
        /// Gets or create Singleton entity. Creates if singleton does not exist
        /// </summary>
        public T Singleton<T>() where T : class
        {
            var instance = Session.GetSingleton(typeof(T));

            if (instance != null)
                return instance as T;
            
            instance = Make<T>();
            
            Session.AddSingleton(typeof(T), instance);

            return Session.GetSingleton(typeof(T)) as T;
        }
        
        public IReadOnlyList<object> AllFabricated()
        {
            return Session.GetAllFabricated();
        }
        
        public void Clear()
        {
            Session.Clear();
        }

        protected TCustomFabricator FabFor<TModel, TCustomFabricator>() 
            where TModel : class
            where TCustomFabricator : ICustomFabricator<TModel>
        {
            return (TCustomFabricator) _support
                .ServiceProvider
                .GetServices<ICustomFabricator>()
                .FirstOrDefault(t => t is ICustomFabricator<TModel>);
        }
        
        private ICustomFabricator<TModel> FabFor<TModel>() where TModel : class
        {
            return (ICustomFabricator<TModel>) _support
                .ServiceProvider
                .GetServices<ICustomFabricator>()
                .FirstOrDefault(t => t is ICustomFabricator<TModel>);
        }

        protected void WithDefault<T>(Action<T> action) where T : new()
        {
            Action<T, Faker> actionWithFaker = (arg1, faker) => action(arg1);
            
            WithDefault(actionWithFaker);
        }
        
        protected void WithDefault<T>(Action<T, Faker> action) where T : new()
        {
            _defaultsWith.AddOrUpdate(typeof(T), action);
        }
    }
}