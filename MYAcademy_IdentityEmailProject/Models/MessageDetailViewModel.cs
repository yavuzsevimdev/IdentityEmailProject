namespace MYAcademy_IdentityEmailProject.Models
{
    public class MessageDetailViewModel
    {
        public int MessageId { get; set; }

        public string Subject { get; set; }
        public string MessageDetail { get; set; }

        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
        public string SenderImageUrl { get; set; }

        public string ReceiverEmail { get; set; }
        public string ReceiverName { get; set; }

        public DateTime SendDate { get; set; }

        public bool IsRead { get; set; }
        public bool IsFavorite { get; set; }

        public string CategoryName { get; set; }
    }
}
