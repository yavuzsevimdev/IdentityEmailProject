namespace MYAcademy_IdentityEmailProject.Models
{
    public class DashboardMessageActivityViewModel
    {
        public List<string> Labels { get; set; } = new();
        public List<int> ReceivedCounts { get; set; } = new();
        public List<int> SentCounts { get; set; } = new();
        public int TotalReceived { get; set; }
        public int TotalSent { get; set; }
    }
}
