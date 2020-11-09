using System;
using System.Collections.Generic;
using AutoFixture;
using Bogus;

namespace Miru.Fabrication
{
    public abstract class CustomFabricator<TModel, TFabricatorFor> : ICustomFabricator<TModel> 
        where TModel : class
        where TFabricatorFor : CustomFabricator<TModel, TFabricatorFor>
    {
        private Action<TModel, Faker> _with = (f, c) => { };
        private Action<TModel, Faker> _finishWith;
        private Action<TModel, Faker> _config;

        protected Fixture Fixture { get; }
        protected Faker Faker { get; }
        
        protected CustomFabricator(FabSupport support)
        {
            Fixture = support.Fixture;
            Faker = support.Faker;
        }
        
        protected void Config(Action<TModel, Faker> action)
        {
            _config = action;
        }
        
        protected void FinishWith(Action<TModel, Faker> action)
        {
            _finishWith = action;
        }
        
        public TModel Make()
        {
            var fabricated = Generate();

            return fabricated;
        }

        private TModel Generate()
        {
            var instance = Fixture.Create<TModel>();

            _config?.Invoke(instance, Faker);
            
            _finishWith?.Invoke(instance, Faker);
            
            _with(instance, Faker);
            
            ClearFinishWith();

            return instance;
        }

        private IEnumerable<TModel> GenerateMany(int count)
        {
            var instances = Fixture.CreateMany<TModel>(count);

            instances.ForEach(m =>
            {
                _config?.Invoke(m, Faker);

                _finishWith?.Invoke(m, Faker);
            });
            
            foreach (var instance in instances)
            {
                _with(instance, Faker);
            }
                
            ClearFinishWith();

            return instances;
        }

        public IEnumerable<TModel> MakeMany(int count)
        {
            var fabricated = GenerateMany(count);
            
            return fabricated;
        }
        
        public TModel Make(Action<TModel> custom)
        {
            var instance = Make();
            
            custom(instance);
            
            return instance;
        }
        
        public TFabricatorFor With(Action<TModel, Faker> func)
        {
            _with += func;
            return (TFabricatorFor) this;
        }
        
        public TFabricatorFor With(Action<TModel> func)
        {
            _with += (model, f) => func(model);
            return (TFabricatorFor) this;
        }
        
        private void ClearFinishWith()
        {
            _with = (model, f) => { };
        }
    }
}