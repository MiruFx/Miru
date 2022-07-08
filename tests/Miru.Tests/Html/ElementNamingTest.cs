using System.Collections.Generic;
using Miru.Domain;
using Miru.Html;
using Miru.Tests.Html.TagHelpers;

namespace Miru.Tests.Html;

public class ElementNamingTest : MiruTagTesting
{
    private readonly ElementNaming _naming;

    public ElementNamingTest()
    {
        _naming = new ElementNaming();
    }

    [Test]
    public void Name_for_form_instance()
    {
        _naming.Form(new AccountLogin.Command()).ShouldBe("account-login-form");
        _naming.Form(new AccountLogin.Result()).ShouldBe("account-login-form");
    }
        
    [Test]
    public void Name_for_form_type()
    {
        _naming.Form(typeof(AccountLogin.Command)).ShouldBe("account-login-form");
        _naming.Form(typeof(AccountLogin.Result)).ShouldBe("account-login-form");
    }
        
    [Test]
    public void Name_for_display_type()
    {
        _naming.Display(typeof(ProductList.Query)).ShouldBe("product-list");
        _naming.Display(typeof(ProductList.Result)).ShouldBe("product-list");
    }
        
    [Test]
    public void Id_for_request()
    {
        _naming.Id(new AccountLogin.Command()).ShouldStartWith("account-login");
        _naming.Id(new ProductList.Query()).ShouldStartWith("product-list");
        
        _naming.Id(new ProductEdit.Command { Id = 2 }).ShouldBe("product-edit_2");
        _naming.Id(new ProductShow.Query { Id = 10 }).ShouldBe("product-show_10");
    }
        
    [Test]
    public void Id_for_entity()
    {
        _naming.Id(new Category { Id = 10 }).ShouldBe("category_10");
        _naming.Id(new Category { Id = 54321 }).ShouldBe("category_54321");
        _naming.Id(new Product { Id = 999333 }).ShouldBe("product_999333");
        _naming.Id(new BoxProduct { Id = 556677 }).ShouldBe("box-product_556677");
    }
    
    [Test]
    public void Id_for_model_expression()
    {
        var result = new ProductList.Result
        {
            InactiveProducts = new()
            {
                new() { AuxiliarProductId = 10 },
                new() { AuxiliarProductId = 11 },
            }
        };
        
        _naming
            .Id(MakeExpression(result, x => x.InactiveProducts[1].AuxiliarProductId))
            .ShouldBe("auxiliar-product-id_11");
    }
    
    public class AccountLogin
    {
        public class Command
        {
        }
            
        public class Result
        {
        }
    }

    public class ProductEdit
    {
        public class Command
        {
            public long Id { get; set; }
        }
    }
    
    public class ProductShow
    {
        public class Query
        {
            public long Id { get; set; }
        }
    }
    
    public class ProductList
    {
        public class Query
        {
        }
            
        public class Result
        {
            public List<Product> InactiveProducts { get; set; } = new();
        }
    }

    public class Category : Entity
    {
    }
        
    public class Product : IEntity
    {
        public long Id { get; set; }
        public long AuxiliarProductId { get; set; }
    }
    
    public class BoxProduct : IEntity
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
    }
}