using System.ComponentModel.DataAnnotations;

namespace CMSApp.ViewModels
{
    public class VerifyEmailViewModel
    {
        [Display(Name = "البريد  الإلكتروني")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
