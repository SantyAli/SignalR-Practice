namespace SignelR_Practice.Models
{
    public class SenderReceiverMessage
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
