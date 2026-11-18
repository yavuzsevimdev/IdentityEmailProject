using MYAcademy_IdentityEmailProject.Entities;

namespace MYAcademy_IdentityEmailProject.Models
{
    public class InboxViewModel
    {
        public List<Message> Messages { get; set; } = new();

        public List<CategoryFilterViewModel> Categories { get; set; } = new();
        public string SortOrder { get; set; } = "newest";
    }

    public class CategoryFilterViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int MessageCount { get; set; } = 0;
    }
}
