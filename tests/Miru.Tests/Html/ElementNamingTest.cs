using Miru.Domain;
using Miru.Html;

namespace Miru.Tests.Html;

public class ElementNamingTest
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
    }
        
    [Test]
    public void Id_for_entity()
    {
        _naming.Id(new Category { Id = 10 }).ShouldBe("category_10");
        _naming.Id(new Category { Id = 54321 }).ShouldBe("category_54321");
        _naming.Id(new Product { Id = 999333 }).ShouldBe("product_999333");
        _naming.Id(new BoxProduct { Id = 556677 }).ShouldBe("box-product_556677");
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

    public class ProductList
    {
        public class Query
        {
        }
            
        public class Result
        {
        }
    }

    public class Category : Entity
    {
    }
        
    public class Product : IEntity
    {
        public long Id { get; set; }
    }
    
    public class BoxProduct : IEntity
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
    }
}