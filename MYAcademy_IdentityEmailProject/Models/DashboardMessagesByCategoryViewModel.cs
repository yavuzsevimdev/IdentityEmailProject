namespace MYAcademy_IdentityEmailProject.Models
{
    public class DashboardMessagesByCategoryViewModel
    {
        public List<MessagesByCategoryItemViewModel> Items { get; set; } = new();

        public int CategoryCount => Items.Count;
    }

    public class MessagesByCategoryItemViewModel
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
    }
}
