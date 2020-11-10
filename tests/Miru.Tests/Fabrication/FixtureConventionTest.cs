using System;
using System.Collections.Generic;
using AutoFixture;
using Bogus;
using Miru.Fabrication.FixtureConventions;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Fabrication
{
    public class FixtureConventionTest
    {
        [Test]
        public void Can_use_a_func_with_faker_for_many_conventions()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.Use(f => "f.Person.FullName").If(_ =>
                {
                    _.IfPropertyNameIs("FullName");
                    _.IfPropertyNameIs("CustomerName");
                });
            });
            
            fixture.Create<Customer>().CustomerName.ShouldBe("f.Person.FullName");
            
            fixture.Create<User>().FullName.ShouldBe("f.Person.FullName");
            
            // should not match
            fixture.Create<User>().NameForUi.ShouldNotBe("f.Person.FullName");
        }
        
        [Test]
        public void An_use_fixed_value_for_many_conventions()
        {
            var expectedName = "Prince Rogers Nelson";
            
            var fixture = CreateFixture(cfg =>
            {
                cfg.Use(expectedName).If(_ =>
                {
                    _.IfPropertyNameEnds("Name");
                });
            });
            
            fixture.Create<Customer>().CustomerName.ShouldBe(expectedName);
            
            fixture.Create<User>().FullName.ShouldBe(expectedName);
            
            // should not match
            fixture.Create<User>().NameForUi.ShouldNotBe(expectedName);
        }
        
        [Test]
        public void When_property_is()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyIs<Customer>(m => m.CustomerName).Use("Customer.FullName");
            });
            
            fixture.Create<Customer>().CustomerName.ShouldBe("Customer.FullName");
            
            fixture.Create<User>().FullName.ShouldNotBe("Customer.FullName");
        }
        
        [Test]
        public void By_type_info()
        {
            var customer = new Customer();
            
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfClass(t => t == typeof(Customer)).Use(customer);
            });

            fixture.Create<User>().ShouldNotBeNull();
            
            fixture.Create<Customer>().ShouldBe(customer);
        }
        
        [Test]
        public void When_class_is()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfClassIs<User>().Ignore();
            });
            
            fixture.Create<Customer>().ShouldNotBeNull();
            
            fixture.Create<User>().ShouldBeNull();
        }
        
        [Test]
        public void When_class_implements_type()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfClassImplements<IEntity>().Ignore();
            });
            
            fixture.Create<Customer>().ShouldBeNull();
            
            fixture.Create<User>().ShouldNotBeNull();
        }
        
        [Test]
        public void By_property_name()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyNameIs("FullName").Use("Joao Silva");
            });
            
            fixture.Create<User>().FullName.ShouldBe("Joao Silva");
            
            // not match
            fixture.Create<Customer>().CustomerName.ShouldNotBe("Joao Silva");
        }
        
        [Test]
        public void By_property_name_start()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyNameStarts("First").Use("Joao Silva");
            });
            
            fixture.Create<Customer>().FirstName.ShouldBe("Joao Silva");
            
            // not match
            fixture.Create<Customer>().CustomerName.ShouldNotBe("Joao Silva");
        }
        
        [Test]
        public void By_property_name_end()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyNameEnds("Name").Use("Edson Arantes do Nascimento");
            });
            
            fixture.Create<Customer>().CustomerName.ShouldBe("Edson Arantes do Nascimento");
        }
        
        [Test]
        public void By_property_type()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyTypeIs<int>().Use(5000);
                cfg.IfPropertyTypeIs<string>().Use("Richard Starkey");
            });
            
            fixture.Create<Customer>().Score.ShouldBe(5000);
            fixture.Create<Customer>().CustomerName.ShouldBe("Richard Starkey");
            
            fixture.Create<User>().AuthAttempts.ShouldBe(5000);
            fixture.Create<User>().FullName.ShouldBe("Richard Starkey");
        }
        
        [Test]
        public void When_property_implements_type_generic_parameter()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyImplements<IEntity>().Use(new Customer
                {
                    CustomerName = "Stefani Joanne Angelina Germanotta"
                });
            });
            
            fixture.Create<User>().Customer.CustomerName.ShouldBe("Stefani Joanne Angelina Germanotta");
        }
        
        [Test]
        public void When_property_implements_type()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyImplements(typeof(IEntity)).Use(new Customer
                {
                    CustomerName = "Stefani Joanne Angelina Germanotta"
                });
            });
            
            fixture.Create<User>().Customer.CustomerName.ShouldBe("Stefani Joanne Angelina Germanotta");
        }
        
        [Test]
        public void When_property_implements_enumerable_of_a_type()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyImplementsEnumerableOf<IEntity>().Ignore();
            });
            
            fixture.Create<Customer>().Orders.ShouldBeNull();

            var dontIgnoreFixture = CreateFixture(cfg => { });
            
            dontIgnoreFixture.Create<Customer>().Orders.ShouldNotBeNull();
        }

        [Test]
        public void When_filter_matches_can_ignore()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfPropertyTypeIs<int>().Ignore();
                
                cfg.IfPropertyNameStarts("First").Ignore();
            });
            
            var user = fixture.Create<User>();
            
            user.AuthAttempts.ShouldBe(default);
            user.FullName.ShouldNotBeEmpty();

            var customer = fixture.Create<Customer>();
            
            customer.Score.ShouldBe(default);
            customer.FirstName.ShouldBeNullOrEmpty();
            customer.CustomerName.ShouldNotBeEmpty();
        }
        
        [Test]
        public void For_property_info_and_fixed_value()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfProperty(p => p.Name == "CustomerName").Use("Joao Carlos Clementoni Silva");
            });

            fixture.Create<Customer>().CustomerName.ShouldBe("Joao Carlos Clementoni Silva");
        }
        
        [Test]
        public void For_property_info_and_faker_value()
        {
            var fixture = CreateFixture(cfg =>
            {
                cfg.IfProperty(p => p.Name == "CustomerName").Use(f => f.Random.Int(1, 1).ToString());
            });

            fixture.Create<Customer>().CustomerName.ShouldBe("1");
        }

        [Test]
        public void Can_ignore_by_property_referencing_a_class()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfPropertyIs<User>(p => p.Customer).Ignore();
            });

            fixture.Create<User>().Customer.ShouldBeNull();
        }

        [Test]
        public void Can_ignore_if_property_type_implements_some_base_generic_type()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfPropertyImplements(typeof(Enumeration<,>)).Ignore();
            });

            fixture.Create<Product>().ProductColor.ShouldBeNull();
        }

        [Test]
        public void Last_configured_filter_should_be_applied_over_first_configured()
        {
            var fixture = CreateFixture(_ =>
            {
                _.IfPropertyNameIs("FirstName").Use("First one");
                
                _.IfPropertyNameIs("FirstName").Use("Last one");
            });

            fixture.Create<Customer>().FirstName.ShouldBe("Last one"); 
        }
        
        [Test]
        public void Last_configured_filter_should_be_applied_over_first_configured_when_different_add_convention_were_called()
        {
            var faker = new Faker();
            
            var fixture = new Fixture()
                .AddConvention(faker, _ =>
                {
                    _.IfPropertyNameIs("FirstName").Use("First one");
                })
                .AddConvention(faker, _ =>
                {
                    _.IfPropertyNameIs("FirstName").Use("Last one");
                });

            fixture.Create<Customer>().FirstName.ShouldBe("Last one"); 
        }
        
        private Fixture CreateFixture(Action<ConventionExpression> cfg) => 
            new Fixture().AddConvention(new Faker(), cfg);

        public interface IEntity
        {
        }
        
        public class Customer : IEntity
        {
            public string FirstName { get; set; }
            public string CustomerName { get; set; }
            public int Score { get; set; }
            public long Id { get; }
            public long Ranking { get; set; }
            
            public IEnumerable<Order> Orders { get; set; }
        }
        
        public class User
        {
            public string FullName { get; set; }
            public string NameForUi { get; set; }
            public int AuthAttempts { get; set; }
            public Customer Customer { get; set; }
        }
        
        public class Order : IEntity
        {
            public long Id { get; }
        }

        public class Product
        {
            public ProductColor ProductColor { get; set; }
        }

        public class ProductColor : Enumeration<ProductColor, int>
        {
        }
        
        public class Enumeration<TEnum, TKeyType>
        {
        }
    }

}