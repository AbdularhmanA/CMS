using System.ComponentModel.DataAnnotations;

namespace CMSApp.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Display(Name = "البريد الإلكتروني")]
        [Required(ErrorMessage = "البريد الالكتروني مطلوب")]
        [EmailAddress]
        public string Email { get; set; }

    
        [Required(ErrorMessage = "كلمة المرور مطلوبة.")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "كلمة المرور الجديدة يجب ان تتكون من 4 إلى 8 رموز")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور الجديدة")]
        [Compare("ConfirmNewPassword", ErrorMessage = "كلمة المرور لاتتطابق.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "تأكيد كلمة المرور مطلوب.")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور الجديدة")]
        public string ConfirmNewPassword { get; set; }


    }
}
