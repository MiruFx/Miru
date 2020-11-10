using System.Collections.ObjectModel;
using AutoFixture;
using Bogus;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Fabrication
{
    public class EntityFrameworkConventionTest
    {
        [Test]
        public void Id_should_be_ignored()
        {
            var fixture = CreateFixture();
            
            fixture.Create<Customer>().Id.ShouldBe(0);
            
            fixture.Create<Order>().Id.ShouldBe(0);
        }
        
        [Test]
        public void Properties_ending_with_id_should_be_ignored()
        {
            var fixture = CreateFixture();
            
            fixture.Create<Order>().CustomerId.ShouldBe(0);
            
            fixture.Create<User>().CustomerId.ShouldBe(0);
        }
        
        [Test]
        public void Properties_as_any_kind_of_list_should_be_ignored()
        {
            var fixture = CreateFixture();
            
            fixture.Create<Order>().Items.ShouldBeNull();
            
            fixture.Create<Customer>().Orders.ShouldBeNull();
        }
        
        private Fixture CreateFixture()
        {
            return new Fixture()
                .OmitRecursion()
                .AddConvention(new Faker(), cfg =>
                {
                    cfg.AddEntityFramework();
                });
        }

        public class Customer : IEntity
        {
            public string FirstName { get; set; }
            public string CustomerName { get; set; }
            public int Score { get; set; }
            public long Id { get; set; }
            
            public Collection<Order> Orders { get; set; }
        }
        
        public class User
        {
            public string FullName { get; set; }
            public string NameForUi { get; set; }
            public int AuthAttempts { get; set; }
            public Customer Customer { get; set; }
            public long CustomerId { get; set; }
        }
        
        public class Order : IEntity
        {
            public long Id { get; set; }
            public Customer Customer { get; set; }
            public long CustomerId { get; set; }
            
            public Collection<OrderItem> Items { get; set; }
        }
        
        public class OrderItem : IEntity
        {
            public long Id { get; set; }
            public Order Order { get; set; }
            public long OrderId { get; set; }
        }
    }
}