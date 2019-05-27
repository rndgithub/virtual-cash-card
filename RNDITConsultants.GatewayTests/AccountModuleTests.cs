using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Nancy;
using Nancy.Testing;
using RNDITConsultants.BusinessLogic;
using RNDITConsultants.Gateway;
using RNDITConsultants.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Gateway.Tests
{
    [TestClass()]
    public class AccountModuleTests
    {
        private Browser _browser;
        private Mock<IAccountController> _accountController;

        [TestInitialize]
        public void Setup()
        {
            _accountController = new Mock<IAccountController>();

            SetupBrowser();
        }

        private void SetupBrowser()
        {
            var logger = new Mock<ILogger>();
            var bootstraper = new ConfigurableBootstrapper
            (
                with => with.Module(new AccountModule(_accountController.Object, logger.Object))
            );

            _browser = new Browser(bootstraper);
        }

        [TestMethod()]
        public void Should_TopUp_Account_Return_Success()
        {
            // Arrange
            var url = "/Account/TopUp/654321/99";

            // Act
            var result = _browser.Put(url, with => with.HttpRequest());

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [TestMethod()]
        public void Should_Withdraw_Account_Return_Success()
        {
            // Arrange
            var url = "/Account/Withdraw/654321/99/1234";

            // Act
            var result = _browser.Put(url, with => with.HttpRequest());

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

    }
}