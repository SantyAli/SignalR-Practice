using SignelR_Practice.Models.View_Model;

namespace SignelR_Practice.Services.Interface
{
    public interface IChatServices
    {
        Task<MessageList> GetChatHistory(string id);
        Task<List<Users>> AllUsers();
        //Task<List<Users>> AllUsers();
    }
}
