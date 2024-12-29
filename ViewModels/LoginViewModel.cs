using System.ComponentModel.DataAnnotations;

namespace CMSApp.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "البريد  الإلكتروني")]
        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "كلمة المرور")]
        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }
    }
}
