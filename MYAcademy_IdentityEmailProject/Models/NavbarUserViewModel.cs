namespace MYAcademy_IdentityEmailProject.Models
{
    public class NavbarUserViewModel
    {
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string Initials { get; set; } = "K";
    }
}
