using CMSApp.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSApp.Models
{
    public class Transaction
    {
        [Key]
        [Required(ErrorMessage = "رقم المعاملة مطلوب.")]
        public string TransactionNumber { get; set; }

        [Required(ErrorMessage = "نوع المعاملة مطلوب.")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "حالة المعاملة مطلوبة.")]
        public string TransactionStatus { get; set; }

        public string TransactionNature { get; set; }
        public string? UserId { get; set; }

        public DateTime ReceptionDate { get; set; }= DateTime.Now;

        public byte[]? AttachmentData { get; set; } 

        public string? Notes { get; set; }

        
        public string? ConductTransaction { get; set; }

        // سبب الرفض
        public string? RejectionReason { get; set; }


        [Required(ErrorMessage = "المستلم مطلوب.")]
        public string? Recipient { get; set; }

        [ForeignKey("UserId")]
      public virtual Users User { get; set; }
        public string? RecipientName { get; set; }  
        public string? RecipientJobTitle { get; set; }  
    }
}
