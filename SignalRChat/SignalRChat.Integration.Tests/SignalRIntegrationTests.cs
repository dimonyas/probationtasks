using System.Web.SessionState;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using SignalRChat.Controllers;
using SignalRChat.Models;
using SpecsFor.Mvc;
using Should;


namespace SignalRChat.Integration.Tests
{
    [TestClass]
    public class SignalRIntegrationTests : SpecsFor.SpecsFor<MvcWebApp>
    {
        private static MvcWebApp _app;

        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _app = new MvcWebApp();
        }

        [TestMethod]
        public void AuthorShouldBeInvalidWhenEmpty()
        {
            _app.NavigateTo<HomeController>(c => c.Index());
            _app.FindFormFor<Message>()
                .Field(f => f.Author).SetValueTo(string.Empty)
                .Field(f => f.Text).SetValueTo("TestMessage");

            _app.Browser.FindElement(By.Id("send-btn")).Click();

            _app.FindFormFor<Message>().Field(f => f.Author).ShouldBeInvalid();
        }

        [TestMethod]
        public void TextFieldShouldBeEmptyAfterSendingMessage()
        {
            _app.NavigateTo<HomeController>(c => c.Index());
            _app.FindFormFor<Message>()
                .Field(f => f.Author).SetValueTo("TestAuthor")
                .Field(f => f.Text).SetValueTo("TestMessage");

            _app.Browser.FindElement(By.Id("send-btn")).Click();

            _app.FindFormFor<Message>().Field(f => f.Author).ShouldNotBeNull();
            _app.Browser.FindElement(By.Id("Text")).Text.ShouldBeSameAs("");
        }
    }
}
