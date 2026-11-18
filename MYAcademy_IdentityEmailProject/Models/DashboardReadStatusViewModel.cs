namespace MYAcademy_IdentityEmailProject.Models
{
    public class DashboardReadStatusViewModel
    {
        public int ReadCount { get; set; }
        public int UnreadCount { get; set; }

        public int TotalCount => ReadCount + UnreadCount;

        public int ReadPercentage => TotalCount > 0 ? (int)Math.Round((double)ReadCount / TotalCount * 100) : 0;

        public int UnreadPercentage => TotalCount > 0 ? 100 - ReadPercentage : 0;
    }
}
