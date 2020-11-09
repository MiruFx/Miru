using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Miru.Html;
using Miru.Testing;
using NUnit.Framework;
using Shouldly;

namespace Miru.Tests.Html
{
    public class FormForTest
    {
        private readonly HtmlGenerator _htmlGenerator;
        private readonly ServiceProvider _sp;

        public FormForTest()
        {
            var services = new ServiceCollection()
                .AddMiruHtml(new HtmlConvention())
                .ReplaceTransient<IAntiforgeryAccessor, StubAntiforgeryAccessor>();
            
            _sp = services.BuildServiceProvider();
            
            _htmlGenerator = new HtmlGenerator(_sp);
        }

        [Test]
        public void Form_should_have_id_named_as_model_type()
        {
            var form = _htmlGenerator.FormFor(new AccountRegister.Command());
            
            form.Id().ShouldBe("account-register-form");
        }

        [Test]
        public void Form_action_should_be_post()
        {
            var form = _htmlGenerator.FormFor(new AccountRegister.Command());
            
            form.Attr("method").ShouldBe("post");
        }
        
        [Test]
        public void Form_should_have_anti_forgery_input()
        {
            var antiForgery = _sp.GetService<IAntiforgeryAccessor>();
            
            var form = _htmlGenerator.FormFor(new AccountRegister.Command());
            var input = form.FirstChild();
            
            input.TagName().ShouldBe("input");
            input.Name().ShouldBe(antiForgery.FormFieldName);
            input.Value().ShouldBe(antiForgery.RequestToken);
        }
    }

    public class AccountRegister
    {
        public class Command
        {
            [FromQuery]
            public string ReturnUrl { get; set; }
        }
    }
}