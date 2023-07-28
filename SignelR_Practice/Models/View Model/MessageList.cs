namespace SignelR_Practice.Models.View_Model
{
    public class MessageList
    {
        public List<AllMessageVM> AllMessages { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
