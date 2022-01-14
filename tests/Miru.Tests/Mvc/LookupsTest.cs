using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Miru.Behaviors.TimeStamp;
using Miru.Databases;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Mvc;
using Miru.Testing;
using Miru.Tests.Databases.EntityFramework;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Mvc
{
    public class LookupsTest
    {
        [Test]
        public void Can_convert_miru_enumeration_to_lookups()
        {
            // arrange
            // act
            var lookups = OrderStatus.GetAll().ToSelectLookups();
            
            // assert
            lookups.ShouldCount(5);
            
            lookups[0].Id.ShouldBe(OrderStatus.Created.Value.ToString());
            lookups[0].Description.ShouldBe(OrderStatus.Created.Name);
            
            lookups[4].Id.ShouldBe(OrderStatus.Received.Value.ToString());
            lookups[4].Description.ShouldBe(OrderStatus.Received.Name);
        }

        [Test]
        public void Can_convert_enum_to_lookups()
        {
            // arrange
            // act
            var lookups = SelectLookups.FromEnum<UserTypes>();
            
            // assert
            lookups.ShouldCount(3);
            
            lookups[0].Id.ShouldBe("1");
            lookups[0].Description.ShouldBe("Administrator");
            
            lookups[2].Id.ShouldBe("3");
            lookups[2].Description.ShouldBe("Management");
        }
        
        [Test]
        public void Can_convert_enumeration_of_T_to_lookups()
        {
            // arrange
            // act
            var lookups = SelectLookups.FromEnum<UserTypes>();
            
            // assert
            lookups.ShouldCount(3);
            
            lookups[0].Id.ShouldBe("1");
            lookups[0].Description.ShouldBe("Administrator");
            
            lookups[2].Id.ShouldBe("3");
            lookups[2].Description.ShouldBe("Management");
        }
        
        [Test]
        public void Can_convert_lookupable_entity_to_lookups()
        {
            // arrange
            var entities = new List<Category>
            {
                new() { Id = 10, Name = "Smartphone" },
                new() { Id = 20, Name = "Laptop" },
                new() { Id = 30, Name = "Watch" }
            };
                
            // act
            var lookups = entities.ToLookups();
            
            // assert
            lookups.ShouldCount(3);
            
            lookups[0].Id.ShouldBe("10");
            lookups[0].Description.ShouldBe("Smartphone");
            
            lookups[2].Id.ShouldBe("30");
            lookups[2].Description.ShouldBe("Watch");
        }

        [Test]
        public void Can_query_lookupable_entity()
        {
            // arrange
            var app = new ServiceCollection()
                .AddMiruApp()
                .AddEfCoreInMemory<FooDbContext>()
                .BuildServiceProvider()
                .GetService<IMiruApp>();
            
            var entities = new List<Category>
            {
                new() { Id = 10, Name = "Smartphone" },
                new() { Id = 20, Name = "Laptop" },
                new() { Id = 30, Name = "Watch" }
            };

            app.WithScope(scope =>
            {
                var db = scope.Get<FooDbContext>();
                db.AddRange(entities);
                db.SaveChanges();
            });

            app.WithScope(scope =>
            {
                var db = scope.Get<FooDbContext>();

                // act
                var lookups = db.Categories.ToLookups();

                // assert
                lookups.ShouldCount(3);

                lookups[0].Id.ShouldBe("10");
                lookups[0].Description.ShouldBe("Smartphone");

                lookups[2].Id.ShouldBe("30");
                lookups[2].Description.ShouldBe("Watch");
            });
        }
        
        public enum UserTypes
        {
            [Display(Name = "Administrator")]
            Admin = 1,
            [Display(Name = "User Support")]
            Support = 2,
            [Display(Name = "Management")]
            Mgmt = 3
        }
        
        public class OrderStatus : Enumeration<OrderStatus>
        {
            public static OrderStatus Created = new(1, "Created");
            public static OrderStatus PendingPayment = new(2, "Pending Payment");
            public static OrderStatus InPreparation = new(3, "In Preparation");
            public static OrderStatus Shipped = new(4, "Shipped");
            public static OrderStatus Received = new(5, "Received");
            
            public OrderStatus(int value, string name) : base(value, name)
            {
            }
        }
        
        public class Category : ILookupableEntity
        {
            public long Id { get; set; }
            public string Name { get; set; }
        }

        public class FooDbContext : DbContext
        {
            public FooDbContext(DbContextOptions<FooDbContext> options) : base(options)
            {
            }

            public DbSet<Category> Categories { get; set; }
        }
    }
}