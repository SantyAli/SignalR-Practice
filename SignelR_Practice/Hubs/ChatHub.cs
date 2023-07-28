using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignelR_Practice.Data;
using SignelR_Practice.Models;

namespace SignelR_Practice.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _contextAccessor;
        public ChatHub(ApplicationDbContext applicationDbContext, IHttpContextAccessor contextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _contextAccessor = contextAccessor;
        }

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToReceiver(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;
            if (receiverId != null && senderId != null)
            {
                await Clients.User(senderId).SendAsync("SendMessage", senderId, message);
                await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);

                var chatMessage = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    Message = message,
                    Reciever = Guid.Parse(receiverId),
                    createdBy = Guid.Parse(senderId),
                    createdDate = DateTime.Now,
                };
                _applicationDbContext.ChatMessages.Add(chatMessage);
                await _applicationDbContext.SaveChangesAsync();
            }


        }
    }
}
