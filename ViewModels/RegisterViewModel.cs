using System.ComponentModel.DataAnnotations;

namespace CMSApp.ViewModels
{
    public class RegisterViewModel
    {

        [Key]
        [Display(Name = "الرقم  الوظيفي")]
        [Required(ErrorMessage = "الرقم الوظيفي مطلوب.")]
        public string UserID { get; set; }

        [Display(Name = "اسم الموظف")]
        [Required(ErrorMessage ="اسم الموظف مطلوب.")]
        public string Name { get; set; }


        [Display(Name = "المسمى  الوظيفي")]
        [Required(ErrorMessage = "المسمى الوظيفي مطلوب.")]
        public string jobtitle { get; set; }

        [Display(Name = "البريد  الإلكتروني")]
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب.")]
        [EmailAddress]
        public string Email { get; set; }


        [Display(Name =  "كلمة المرور")]
        [Required(ErrorMessage = "كلمةا لمرور مطلوبه.")]
        [StringLength(8, MinimumLength = 4, ErrorMessage = "كلمة المرور يجب ان تكون من 4 إلى 8 رموز")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword", ErrorMessage = "كلمةالمرور لاتتطابق.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "تأكيد كلمةالمرور مطلوب.")]
        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        public string ConfirmPassword { get; set; }
    }
}
