using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CMSApp.ViewModels
{
    public class CreateTansactionsViewModel
    {
        [Key]
        [Required(ErrorMessage = "رقم المعاملة مطلوب.")]
        public string TransactionNumber { get; set; }  

        [Required(ErrorMessage = "نوع المعاملة مطلوب.")]
        public string TransactionType { get; set; } 

        [Required(ErrorMessage = "حالة المعاملة مطلوبة.")]
        public string TransactionStatus { get; set; } 

        [Required(ErrorMessage = "طبيعة المعاملة مطلوبة.")]
        public string TransactionNature { get; set; }  

        [Required(ErrorMessage = "جهة المستقبل مطلوبة.")]
        public string ?SelectedRecipientId { get; set; }

        public string? RecipientName { get; set; } 

        public string? RecipientJobTitle { get; set; } 
        public IFormFile? Attachments { get; set; } 

        [Display(Name = "ملاحظات")]
        public string? Notes { get; set; }  

        public List<SelectListItem> AvailableRecipients { get; set; } = new List<SelectListItem>();
    }
}
