using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        public void BroadcastMessage(Message message)
        {
            message.Time = DateTimeOffset.Now.DateTime.ToLongTimeString();
            Clients.All.broadcastMessage(message);
        }
    }
}