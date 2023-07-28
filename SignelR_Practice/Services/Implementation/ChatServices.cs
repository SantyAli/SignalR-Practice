using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignelR_Practice.Data;
using SignelR_Practice.Models.View_Model;
using SignelR_Practice.Services.Interface;
using System.Security.Claims;

namespace SignelR_Practice.Services.Implementation
{
    public class ChatServices : IChatServices
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public ChatServices(IHttpContextAccessor contextAccessor, UserManager<IdentityUser> userManager, ApplicationDbContext applicationDbContext)
        {
            _contextAccessor = contextAccessor;
            _userManager = userManager;
            _applicationDbContext = applicationDbContext;
        }

        public async Task<MessageList> GetChatHistory(string id)
        {
            var chatList = new MessageList();
            Guid? senderId = Guid.Parse(GetLoggedInUserId());
            var recieverId = Guid.Parse(id);
            if (senderId != Guid.Empty)
            {
                chatList.AllMessages = await _applicationDbContext.Notifications
                                                 .Where(x =>
                                                 (x.RecieverId == recieverId && x.SenderId == senderId)
                                                 || (x.RecieverId == senderId && x.SenderId == recieverId))
                                                 .OrderByDescending(x => x.createdDate)
                                                 .Skip(0).Take(20)
                                                 .Select(x => new AllMessageVM
                                                 {
                                                     ReceiverId = x.RecieverId,
                                                     SenderId = x.SenderId,
                                                     IsLeft = senderId == x.SenderId ? 1 : 0,
                                                     createdDate = x.createdDate,
                                                     Message = x.NotificationMessage
                                                 })
                                                 .OrderBy(x => x.createdDate)
                                                 .ToListAsync();
            }
            chatList.ReceiverId = recieverId;
            return chatList;
        }

        public async Task<List<Users>> AllUsers()
        {
            var senderId = Guid.Parse(GetLoggedInUserId());

            return await _userManager.Users
                .Where(x => _applicationDbContext.senderReceiverMessages
                .Any(s =>
                (s.SenderId == senderId && s.ReceiverId.ToString() == x.Id)
                || (s.ReceiverId == senderId && s.SenderId.ToString() == x.Id)))
                .Select(x => new Users
                {
                    Id = x.Id,
                    Name = x.UserName
                })
                .ToListAsync();
        }
        private string GetLoggedInUserId()
        {
            return _contextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
