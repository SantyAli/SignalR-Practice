namespace SignelR_Practice.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public Guid Reciever { get; set; }
        public string Message { get; set; }
        public DateTime createdDate { get; set; }
        public Guid createdBy { get; set; }

    }
}
