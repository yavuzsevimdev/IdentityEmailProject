using System.ComponentModel.DataAnnotations;

namespace MYAcademy_IdentityEmailProject.Models
{
    public class MyProfileViewModel
    {
        public string? Id { get; set; }

        [Display(Name = "Ad")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
        public string? Name { get; set; }

        [Display(Name = "Soyad")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
        public string? Surname { get; set; }

        [Display(Name = "Kullanıcı Adı")]
        [StringLength(50, ErrorMessage = "Kullanıcı adı en fazla 50 karakter olabilir.")]
        public string? UserName { get; set; }

        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Açıklama")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string? Description { get; set; }

        [Display(Name = "Profil Fotoğrafı URL")]
        [Url(ErrorMessage = "Geçerli bir profil fotoğrafı URL adresi giriniz.")]
        public string? ImageUrl { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [Compare(nameof(NewPassword),
            ErrorMessage = "Şifreler eşleşmiyor.")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
    }
}