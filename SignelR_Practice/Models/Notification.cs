namespace SignelR_Practice.Models
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecieverId { get; set; }
        public string NotificationMessage { get; set; }
        public DateTime createdDate { get; set; }
        public Guid SenderId { get; set; }

    }
}
