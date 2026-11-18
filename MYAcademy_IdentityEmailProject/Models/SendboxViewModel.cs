using MYAcademy_IdentityEmailProject.Entities;

namespace MYAcademy_IdentityEmailProject.Models
{
    public class SendboxViewModel
    {
        public List<Message> Messages { get; set; } = new();

        public List<CategoryFilterViewModel> Categories { get; set; } = new();
        public string SortOrder { get; set; } = "newest";
    }
}
