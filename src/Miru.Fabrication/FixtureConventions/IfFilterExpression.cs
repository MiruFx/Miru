using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Miru.Fabrication.FixtureConventions
{
    public class IfFilterExpression
    {
        public Func<PropertyInfo, bool> IfProperty { get; }
        
        public Func<TypeInfo, bool> IfClass { get; }

        public bool IsForClass => IfClass != null;
        
        public bool IsForProperty => IfProperty != null;
        
        internal Func<object> UseValue { get; private set; }
        
        internal bool ShouldIgnore { get; private set; }

        private readonly Faker _faker;

        public IfFilterExpression(Func<PropertyInfo, bool> ifProperty, Faker faker)
        {
            _faker = faker;
            IfProperty = ifProperty;
        }
        
        public IfFilterExpression(Func<TypeInfo, bool> ifClass, Faker faker)
        {
            _faker = faker;
            IfClass = ifClass;
        }

        public void Use(object useValue)
        {
            UseValue = () => useValue;
        }
        
        public void Use(Func<object> useValue)
        {
            UseValue = useValue;
        }
        
        public void Use(Func<Faker, object> useValue)
        {
            UseValue = () => useValue(_faker);
        }
        
        public void Ignore()
        {
            ShouldIgnore = true;
        }
    }
}