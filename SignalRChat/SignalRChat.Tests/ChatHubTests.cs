using System;
using System.Dynamic;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SignalRChat.Hubs;
using SignalRChat.Models;

namespace SignalRChat.Tests
{
    [TestClass]
    public class ChatHubTests
    {
        [TestMethod]
        public void HubBroadcastMessageMethodCalled()
        {
            bool broadcastMessageCalled = false;
            ChatHub hub = new ChatHub();
            var mockClients = new Mock<IHubCallerConnectionContext<dynamic>>();
            hub.Clients = mockClients.Object;
            dynamic all = new ExpandoObject();
            all.broadcastMessage = new Action<Message>(message =>
            {
                broadcastMessageCalled = true;
            });
            mockClients.Setup(m => m.All).Returns((ExpandoObject) all);
            hub.BroadcastMessage(new Message
            {
                Author = "Test Author",
                Time = DateTimeOffset.Now.DateTime.ToLongTimeString(),
                Text = "TestText"
            });
            Assert.IsTrue(broadcastMessageCalled);
        }
    }
}
