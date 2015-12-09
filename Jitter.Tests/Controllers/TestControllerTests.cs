using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using Jitter.Controllers;

namespace Jitter.Tests.Controllers
{
    [TestClass]
    public class TestControllerTests
    {
        [TestMethod]
        public void TestControllerEnsureICanCallGetAction()
        {
            // Arrange
            TestController my_controller = new TestController();
            string expected_output = "Hello, is it me you're looking for?";

            // Act
            string actual_output = my_controller.Get();

            // Assert
            Assert.AreEqual(expected_output, actual_output);
        }
    }
}
