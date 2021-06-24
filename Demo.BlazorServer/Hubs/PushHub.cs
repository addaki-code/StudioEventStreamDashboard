using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Demo.BlazorServer.Hubs
{
    public class PushHub : Hub
    {
        public async Task RequestRedraw()
        {
            await Clients.All.SendAsync("Redraw");
        }
    }
}
