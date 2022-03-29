using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Miru.Domain;
using Miru.Fabrication;
using Miru.Fabrication.FixtureConventions;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Fabrication
{
    public class FabricationTest
    {
        private ThisFabricator _fabricator;
        private ServiceProvider _sp;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var services = new ServiceCollection()
                .AddFabrication<ThisFabricator>(_ =>
                {
                    _.IfPropertyIs<Address>(m => m.City)
                        .Use("City from FabricatorFor");
                    
                    _.IfProperty(p => p.Name.Equals("Name"))
                        .Use("Name from Factory");
                    
                    _.IfPropertyIs<Category>(m => m.Name)
                        .Use("Name from Factory");
                    
                    _.IfPropertyIs<Store>(m => m.Company)
                        .Ignore();
                    
                    _.IfPropertyIs<Store>(m => m.Name)
                        .Use("Name From StoreFactory");
                });
                
            _sp = services.BuildServiceProvider();
                
            _fabricator = _sp.GetService<ThisFabricator>();
        }

        [SetUp]
        public void Setup()
        {
            _fabricator.Clear();
        }
        
        [Test]
        public void Entity_should_make_with_id_0()
        {
            var category = _fabricator.Make<Category>();
            
            category.Id.ShouldBe(0);
        }

        [Test]
        public void Reference_to_an_entity_id_should_be_0()
        {
            var product = _fabricator.Make<Product>();
            
            product.Category.ShouldNotBeNull();
            product.CategoryId.ShouldBe(0);
            
            product.Category.Store.ShouldNotBeNull();
            product.Category.StoreId.ShouldBe(0);
        }
        
        [Test]
        public void Property_list_of_entities_should_be_empty()
        {
            var category = _fabricator.Make<Category>();
                
            category.Products.ShouldBeNull();
        }
        
        [Test]
        public void Property_referencing_an_entity_id_should_be_empty()
        {
            _fabricator.Make<Product>().CategoryId.ShouldBe(0);
            
            _fabricator.Make<Category>().StoreId.ShouldBe(0);
        }

        [Test]
        public void When_making_entity_then_reference_to_other_entities_should_be_singleton()
        {
            var product1 = _fabricator.Make<Product>();

            var product2 = _fabricator.Make<Product>();
            
            product1.Category.ShouldBe(product2.Category);
        }

        [Test]
        public void Singleton_should_be_unique()
        {
            var category1 = _fabricator.Singleton<Category>();
            
            var category2 = _fabricator.Singleton<Category>();
            
            var category3 = _fabricator.Singleton<Category>();
            
            var category4 = _fabricator.Make<Product>().Category;

            category1.ShouldBe(category2);
            
            category1.ShouldBe(category3);
            
            category1.ShouldBe(category4);
        }

        [Test]
        public void Make_should_create_new_and_not_reuse_singleton()
        {
            var category1 = _fabricator.Singleton<Category>();

            var category2 = _fabricator.Make<Category>();
            
            var category3 = _fabricator.Make<Category>();
            
            category1.ShouldNotBe(category2);
            
            category1.ShouldNotBe(category3);
        }
        
        [Test]
        public void Should_return_custom_fabricator_implementation_when_resolving_interface()
        {
            _sp.GetService<ICustomFabricator<Category>>().ShouldBeOfType<CategoryFabricator>();
            
            _sp.GetService<ICustomFabricator<Product>>().ShouldBeNull();
        }
        
        [Test]
        public void Can_configure_factory_for_a_type_with_rules()
        {
            var category1 = _fabricator.Make<Category>();
            
            var category2 = _fabricator.Make<Category>();
            
            category1.Name.ShouldBe("Name from Factory");
            
            category2.Name.ShouldBe("Name from Factory");
        }

        [Test]
        public void Custom_factory_with_custom_finalizer()
        {
            var category1 = _fabricator.Categories.With(m => m.IsActive = true).Make();
            
            category1.IsActive.ShouldBeTrue();
            
            var category2 = _fabricator.Categories.Make(m => m.IsActive = true);
            
            category2.IsActive.ShouldBeTrue();
        }
        
        [Test]
        public void Should_use_reference_ids_configuration()
        {
            var category = _fabricator.Make<Category>();
            
            category.StoreId.ShouldBe(0);
        }
        
        [Test]
        public void Should_use_reference_singleton_configuration()
        {
            var category1 = _fabricator.Make<Category>();
            
            var category2 = _fabricator.Make<Category>();

            category1.Store.ShouldBe(category2.Store);
        }

        [Test]
        public void Can_use_composer_methods()
        {
            // arrange
            var category = _fabricator.Make<Category>();

            var madeStore = _fabricator.Make<Store>();

            // act
            var categoryWithMadeStore = _fabricator.Categories.WithStore(madeStore).Make();
            
            // assert
            categoryWithMadeStore.Store.ShouldBe(madeStore);
            
            madeStore.ShouldNotBe(category.Store);
            
            category.Store.ShouldBe(_fabricator.Singleton<Store>());
        }
        
        [Test]
        public void Can_use_composer_methods_to_create_many()
        {
            var category1 = _fabricator.Make<Category>();

            var someOtherStore = _fabricator.Make<Store>();

            var someOtherCategories = _fabricator.Categories.WithStore(someOtherStore).MakeMany(5);
            
            category1.Store.ShouldBe(_fabricator.Singleton<Store>());
            
            someOtherStore.ShouldNotBe(category1.Store);

            someOtherCategories.ForEach(m => m.Store.ShouldBe(someOtherStore));
        }

        [Test]
        [Ignore("Decide Fabricated vs Singletons. If AllFabricated should return singletons as well")]
        public void Should_store_all_fabricated_objects()
        {
            // making a product demands 
            var product1 = _fabricator.Make<Product>();
            var products = _fabricator.MakeMany<Product>(2);

            var fabricated = _fabricator.AllFabricated();
            
            fabricated[0].ShouldBeOfType<Address>();
            fabricated[1].ShouldBe(_fabricator.Singleton<Store>());
            fabricated[2].ShouldBe(_fabricator.Singleton<Category>());
            fabricated[3].ShouldBe(product1);
            fabricated[4].ShouldBe(products.At(0));
            fabricated[5].ShouldBe(products.At(1));
        }

        [Test]
        public void Can_use_custom_fabricator_not_mapped_on_app_fabricator()
        {
            var store = _fabricator.Make<Store>();
            
            store.Name.ShouldBe("Name From StoreFactory");
        }
        
        [Test]
        public void Can_clear_fabricated_store()
        {
            // arrange
            _fabricator.Make<Product>();
            _fabricator.AllFabricated().Count.ShouldBe(1);

            // act
            _fabricator.Clear();
            
            // assert
            _fabricator.AllFabricated().ShouldBeEmpty();
        }

        [Test]
        public void Fabricator_should_be_singleton()
        {
            _sp.GetService<ThisFabricator>().ShouldBe(_fabricator);
            
            _sp.GetService<Fabricator>().ShouldBe(_fabricator);
        }

        [Test]
        public void If_a_type_has_a_fabricatorfor_it_then_should_use_it_for_singletons()
        {
            var product = _fabricator.Make<Product>();

            var category = _fabricator.Singleton<Category>();
            
            product.Category.ShouldBe(category);
            
            category.Name.ShouldBe("Name from Factory");
        }

        [Test]
        public void If_type_is_not_entity_and_has_fabricatorfor_should_use_config_but_not_singleton()
        {
            var store1 = _fabricator.Make<Store>();
            
            var store2 = _fabricator.Make<Store>();

            store1.Address.City.ShouldBe("City from FabricatorFor");
            
            store2.Address.City.ShouldBe("City from FabricatorFor");

            store1.Address.ShouldNotBe(store2.Address);
        }

        [Test]
        public void Can_handle_entity_that_references_entities_with_circular_reference()
        {
            var category = _fabricator.Make<Category>();
            var store = category.Store;
            var storeInsurance = store.StoreInsurance;
            
            category.Store.ShouldBe(store);
            store.StoreInsurance.ShouldBe(storeInsurance);
            
            store.StoreInsurance.Store.ShouldBeNull();
        }
        
        [Test]
        public void Can_create_entity_based_in_fabricator_default_configuration()
        {
            var customer1 = _fabricator.Make<Customer>();
            customer1.Status.ShouldBe("regular-buyer");
            customer1.IsActive.ShouldBeTrue();
            customer1.Address.ShouldNotBeNull();
            customer1.Rank.ShouldBe(10);
            
            var customer2 = _fabricator.Make<Customer>();
            customer2.Status.ShouldBe("regular-buyer");
            customer2.IsActive.ShouldBeTrue();
            customer2.Address.ShouldNotBeNull();
            customer2.Rank.ShouldBe(10);
            
            var paymentProvider = _fabricator.Make<PaymentProvider>();
            paymentProvider.Name.ShouldBeOneOf("PayPal", "Stripe");
        }
        
        [Test]
        public void Should_cast_implicit_operator_for_custom_fabricators()
        {
            Category category = _fabricator.Categories.With(m => m.IsActive = true); // no need of .Make()
            
            category.IsActive.ShouldBeTrue();
        }
        
        [Test]
        [Ignore("Not implemented yet")]
        public void When_default_specified_should_use_it_when_building_object()
        {
            var order1 = _fabricator.Make<Order>();
            order1.Customer.Status.ShouldBe("regular-buyer");
            
            var order2 = _fabricator.Make<Order>();
            order2.Customer.Status.ShouldBe("regular-buyer");
        }
        
        public class ThisFabricator : Fabricator
        {
            public ThisFabricator(FabSupport support) : base(support)
            {
                support.Fixture.AddConvention(_ =>
                {
                    _.IfPropertyNameIs(nameof(Customer.Rank)).Use(10);
                });
                
                WithDefault<Customer>(x =>
                {
                    x.Status = "regular-buyer";
                    x.IsActive = true;
                });

                WithDefault<PaymentProvider>((x, faker) =>
                {
                    x.Name = faker.PickRandom("PayPal", "Stripe");
                });
            }

            public CategoryFabricator Categories => FabFor<Category, CategoryFabricator>();
        }

        public class CategoryFabricator : CustomFabricator<Category, CategoryFabricator>
        {
            public CategoryFabricator(FabSupport support) : base(support)
            {
            }
            
            public CategoryFabricator WithStore(Store store) => With(p => p.Store = store);
        }

        public class Company : Entity
        {
        }
        
        public class Store : Entity
        {
            public string Name { get; set; }
            public Address Address { get; set; }
            public Company Company { get; set; }
            public StoreInsurance StoreInsurance { get; set; }
        }

        public class StoreInsurance : Entity
        {
            public Store Store { get; set; }
        }
        
        public class Product : Entity
        {
            public string Name { get; set; }
        
            public long CategoryId { get; set; }
            public Category Category { get; set; }
        }
        
        public class Category : Entity
        {
            public long StoreId { get; set; }
            public Store Store { get; set; }

            public string Name { get; set; }
            public bool IsActive { get; set; }
        
            public List<Product> Products { get; set; }
        }
        
        public class Address
        {
            public string City { get; set; }
            public string Country { get; set; }
        }

        public class Customer
        {
            public bool IsActive { get; set; }
            public Address Address { get; set; }
            
            // should set as FixtureConvention
            public int Rank { get; set; }
            
            // usually status would be strong typed. it is string just for the test sake
            public string Status { get; set; } 
        }
        
        public class PaymentProvider
        {
            public string Name { get; set; } 
        }

        public class Order
        {
            public Customer Customer { get; set; }
        }
    }
}