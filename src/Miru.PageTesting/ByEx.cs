using OpenQA.Selenium;

namespace Miru.PageTesting
{
    public class ByEx : By
    {
        public static By Title(string text)
        {
            var xpath = $@"
//h1[text()='{text}'] | 
//h2[text()='{text}'] | 
//h3[text()='{text}'] | 
//h4[text()='{text}'] | 
//h5[text()='{text}'] | 
//h6[text()='{text}']";

            return By.XPath(xpath);
        }

        public static By Submit() => By.CssSelector("input[type='submit']");
    }
}