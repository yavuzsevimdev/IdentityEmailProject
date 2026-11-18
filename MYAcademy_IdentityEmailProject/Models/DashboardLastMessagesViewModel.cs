namespace MYAcademy_IdentityEmailProject.Models
{
    public class DashboardLastMessagesViewModel
    {
        public List<DashboardLastMessageItemViewModel> RecentMessages { get; set; } = new();
    }

    public class DashboardLastMessageItemViewModel
    {
        public int MessageId { get; set; }

        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Initials { get; set; }
        public string? ImageUrl { get; set; }

        public string Subject { get; set; }
        public DateTime SendDate { get; set; }

        public bool IsIncoming { get; set; }
        public bool IsRead { get; set; }
        public bool IsFavorite { get; set; }

        public string AvatarClass { get; set; }
    }
}
