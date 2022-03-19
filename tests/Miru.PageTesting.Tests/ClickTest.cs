namespace Miru.PageTesting.Tests;

public class ClickLinkTest
{
    [TestFixture]
    public class ClickLinkTestChrome : Tests
    {
        public ClickLinkTestChrome() => _ = DriverFixture.Get.Value.ForChrome();
    }
    
    [TestFixture]
    public class ClickLinkTestFirefox : Tests
    {
        public ClickLinkTestFirefox() => _ = DriverFixture.Get.Value.ForFirefox();
    }
        
    public abstract class Tests
    {
        protected DriverFixture _;

        [Test]
        public void Can_click_in_a_link()
        {
            _.HtmlIs("<a href='contact.html'>Contact</a>");
                
            _.Page.ClickLink("Contact");
    
            _.Page.Url.ShouldBe(_.Page.UrlFor("/contact.html"));
        }
    
        [Test]
        public void Can_click_in_a_link_with_a_button_inside()
        {
            _.HtmlIs("<a href='delete.html'><button>Delete</button></a>");
                
            _.Page.ClickLink("Delete");
    
            _.Page.Url.ShouldBe(_.Page.UrlFor("/delete.html"));
        }
            
        [Test]
        public void Throw_exception_if_cannot_find_link()
        {
            _.HtmlIs("<a href='contact.html'>Contact</a>");
                
            Should
                .Throw<PageTestException>(() => _.Page.ClickLink("Link Does Not Exist"))
                .Message
                .ShouldContain("By.LinkText: Link Does Not Exist");
        }
            
        [Test]
        public void Can_click_by_query()
        {
            _.HtmlIs("<a href='/cart'><button id='cart'>Cart<button></a>");
                
            _.Page.Click(By.Id("cart"));
    
            _.Page.Url.ShouldBe(_.Page.UrlFor("/cart"));
        }
            
        [Test]
        public void Throw_exception_if_more_than_one_clickable_element_was_found()
        {
            _.HtmlIs(@"
    Reviews from <a href='/reviews/by/women'>Women</a>
    <div id='filter'>
        <a href='/products/women'>Women</a>
    </div>");
    
            var ex = Should.Throw<PageTestException>(() => _.Page.ClickLink("Women"));
            ex.InnerException.ShouldBeOfType<ManyElementsFoundException>();
            ex.InnerException.Message.ShouldContain("Many elements found for 'By.LinkText: Women'");
        }
            
        [Test]
        public void Can_click_in_a_link_not_visible_at_the_scroll_position()
        {
            _.HtmlIs($@"
{100.Times(i => $"<p>{i}</p>")}
<a href='/cart'><button id='cart'>Cart<button></a>");
                
            _.Page.ClickLink("Cart");
    
            _.Page.Url.ShouldBe(_.Page.UrlFor("/cart"));
        }
    }
}