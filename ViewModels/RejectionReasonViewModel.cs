using System.ComponentModel.DataAnnotations;

namespace CMSApp.ViewModels
{
    public class RejectionReasonViewModel
    {
        public string TransactionNumber { get; set; }

        [Required(ErrorMessage = "يرجى إدخال سبب الرفض")]
        [Display(Name = "سبب الرفض")]
        public string RejectionReason { get; set; } 

    }
}
