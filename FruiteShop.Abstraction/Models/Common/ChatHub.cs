using FruiteShop.Abstraction.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FruiteShop.Abstraction.Models.Common
{
    public class ChatHub : Hub
    {
        private readonly IHubContext<ChatHub> _hubContext;

        //public ChatHub(IHubContext<ChatHub> hubContext)
        //{
        //    _hubContext = hubContext;
        //}

        private readonly HubService _hubService;

        public ChatHub(HubService hubService, IHubContext<ChatHub> hubContext)
        {
            _hubService = hubService;
            _hubContext = hubContext;
        }

        public override async Task OnConnectedAsync()
        {

            //Connected
            var objUserId = Context.GetHttpContext().Request.Query["userId"];
            int userId = 0;

            int.TryParse(objUserId, out userId);

            _hubService.addConnectDetailsToDict(userId, Context.ConnectionId);

            await base.OnConnectedAsync();
        }

        public string GetConnectionId() => Context.ConnectionId;

        public async Task SendMessage(string user, string message,string recipientId)
        {
            try
            {
                await Clients.Client(recipientId).SendAsync("ReceiveChat", user, message);
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
            //string connectionId = _hubService.getConnectionIdByUserId(recipientId);


            //await _hubContext.Clients.All.SendAsync("ReceiveChat", user, message);

            //await Clients.All.SendAsync("ReceiveChat", user, message);
        }

        public async Task SendUserStatusChangeNotification(string status,int userId)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("ReceiveUserStatus", status,userId);
            }
            catch(Exception ex)
            {
                string exc = ex.Message;
            }
            
        }

        public string getConnectIdOfUser(int userId)
        {
            var connectionId = _hubService.getConnectionIdByUserId(userId);
            return connectionId;
        }

        
    }
}
