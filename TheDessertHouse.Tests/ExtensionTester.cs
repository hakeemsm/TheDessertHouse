using System.Web.Mvc;
using System.Web.Mvc.Html;
using NUnit.Framework;
using TheDessertHouse.Domain;
using TheDessertHouse.Web;
using TheDessertHouse.Web.Controllers;
using TheDessertHouse.Web.Models;

namespace TheDessertHouse.Tests
{
    [TestFixture]
    public class ExtensionTester
    {
        [Test]
        public void WithLabel_Returns_Html_With_Label_Tag()
        {
            
            var htmlHelper=new HtmlHelper<UserInformationView>(new ViewContext(),new ViewPage<UserInformationView>());
            //htmlHelper.GetTextBoxWithLabelFor(m=>m.UserName)
            
        }
    }

    internal class TextBox
    {
        
    }
}
