using System;
using Miru.Testing;

namespace Miru.PageTesting
{
    public static class PageTestFixtureExtensions
    {
        public static void Form<TModel>(this PageTestFixture fixture, Action<PageElement<TModel>, TModel> action) where TModel : class
        {
            fixture.Form<TModel>(f =>
            {
                var fabricated = fixture.Make<TModel>();
                action(f, fabricated);
            });
        }
    }
}