using Microsoft.AspNetCore.SignalR;
using tpiPracticeClasses;

namespace tpiPracticeASPNET.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendGroupchatAddedMessage(Groupchat groupchat)
        {
            await Clients.All.SendAsync("GroupchatAdded", groupchat);
        }

        public async Task SendGroupchatEditedMessage(Groupchat groupchat)
        {
            await Clients.All.SendAsync("GroupchatEdited", groupchat);
        }

        public async Task SendGroupchatRemovedMessage(Groupchat groupchat)
        {
            await Clients.All.SendAsync("GroupchatRemoved", groupchat);
        }

        public async Task SendMsgAddedMesage(Message message)
        {
            await Clients.All.SendAsync("MsgAdded", message);
        }

        public async Task SendMsgEditedMessage(Message message)
        {
            await Clients.All.SendAsync("MsgEdited", message);
        }

        public async Task SendMsgRemovedMessage(Message message)
        {
            await Clients.All.SendAsync("MsgRemoved", message);
        }
    }
}
