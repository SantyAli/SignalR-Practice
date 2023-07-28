namespace SignelR_Practice.Models.View_Model
{
    public class AllMessageVM
    {
        public Guid ReceiverId { get; set; }
        public Guid SenderId { get; set; }
        public string Message { get; set; }
        public DateTime createdDate { get; set; }
        public int IsLeft { get; set; }

    }
}
