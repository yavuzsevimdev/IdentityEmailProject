namespace MYAcademy_IdentityEmailProject.Models
{
    public class DashboardCategoryDistributionViewModel
    {
        public List<DashboardDistributionItemViewModel> Items { get; set; } = new();
    }

    public class DashboardDistributionItemViewModel
    {
        public string Label { get; set; }
        public int Count { get; set; }
        public string Color { get; set; }
        public int Percentage { get; set; }
    }
}