using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignelR_Practice.Data;
using SignelR_Practice.Models;

namespace SignelR_Practice.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private static readonly Dictionary<string, string> ConnectedUsersCache = new Dictionary<string, string>();
        //private static readonly TimeSpan CacheExpirationTime = TimeSpan.FromMinutes(2880); // Adjust the expiration time as needed
        private static bool isCacheInitialized = false;


        public NotificationHub(ApplicationDbContext applicationDbContext, UserManager<IdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
        }

        public override async Task OnConnectedAsync()
        {
            if (!isCacheInitialized)
            {
                _applicationDbContext.senderReceiverMessages.ToList().ForEach(x =>
                {
                    var cacheKey = $"IsSenderReceiverExist_{x.ReceiverId}_{x.SenderId}";
                    ConnectedUsersCache[cacheKey] = true.ToString();
                });
                isCacheInitialized = true;
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotificationToReceiver(string receivers, bool isUserInChat, string userMessage)
        {
            var senders = Context.UserIdentifier;

            if (receivers != null && senders != null && isUserInChat)
            {
                if (!ConnectedUsersCache.ContainsKey(receivers) || !ConnectedUsersCache.ContainsKey(senders))
                {
                    var receiver = await _userManager.FindByIdAsync(receivers);
                    if (receiver == null)
                        return;

                    ConnectedUsersCache[receiver.Id] = senders;
                    ConnectedUsersCache[senders] = receiver.Id;
                }

                var receiverId = Guid.Parse(ConnectedUsersCache[senders]);
                var senderId = Guid.Parse(ConnectedUsersCache[receivers]);
                await GetSenderAndReceiver(receiverId, senderId);

                bool isMessageSaved = await AddingEntitytoDB(receiverId, senderId, userMessage);
                if (isMessageSaved)
                {
                    await Clients.User(receivers).SendAsync("ReceiveNotification", userMessage, 1);
                    await Clients.Users(senders).SendAsync("ReceiveNotification", userMessage, 2);
                }
                else
                {
                    await Clients.User(receivers).SendAsync("ReceiveNotification", $"Message Not Sent {userMessage}");
                    await Clients.Users(senders).SendAsync("ReceiveNotification", $"Message Not Sent {userMessage}");
                }
            }
        }

        private async Task<bool> GetSenderAndReceiver(Guid receiverId, Guid senderId)
        {
            var cacheKey = $"IsSenderReceiverExist_{receiverId}_{senderId}";

            if (ConnectedUsersCache.TryGetValue(cacheKey, out var cachedValue))
                return true;

            var result = await IsSenderAndReceiverExists(receiverId, senderId);
            ConnectedUsersCache[cacheKey] = result.ToString();
            return result;
        }

        private async Task<bool> IsSenderAndReceiverExists(Guid receiverId, Guid senderId)
        {
            var senderReceiverMessageEntity = await _applicationDbContext.senderReceiverMessages
                .FirstOrDefaultAsync(s => s.ReceiverId == receiverId && s.SenderId == senderId);

            if (senderReceiverMessageEntity == null)
            {
                var senderReceiverMessage = new SenderReceiverMessage
                {
                    Id = Guid.NewGuid(),
                    ReceiverId = receiverId,
                    SenderId = senderId
                };
                await _applicationDbContext.senderReceiverMessages.AddAsync(senderReceiverMessage);
                return true;
            }

            return false;
        }

        private async Task<bool> AddingEntitytoDB(Guid receiverId, Guid senderId, string message)
        {
            try
            {
                var notification = new Notification
                {
                    Id = Guid.NewGuid(),
                    NotificationMessage = message,
                    RecieverId = receiverId,
                    SenderId = senderId,
                    createdDate = DateTime.UtcNow,
                };
                await _applicationDbContext.Notifications.AddAsync(notification);
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
