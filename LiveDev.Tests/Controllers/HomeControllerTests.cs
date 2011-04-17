using System.Web.Mvc;
using LiveDev.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiveDev.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_ShowTheWebDescription()
        {
            var controller = new HomeController();

            var result = controller.Index() as ViewResult;

            Assert.IsNotNull(result.ViewBag.WebDescription);
        }
    }
}
