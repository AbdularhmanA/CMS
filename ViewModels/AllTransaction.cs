using System.ComponentModel.DataAnnotations;

namespace CMSApp.ViewModels
{
    public class AllTransaction
    {
  
        public string TransactionNumber { get; set; }

        public string TransactionType { get; set; }

     
        public string TransactionStatus { get; set; }

        public string TransactionNature { get; set; }
        public string? UserId { get; set; }

        public string? ConductTransaction { get; set; }


        public DateTime ReceptionDate { get; set; } = DateTime.Now;

        public string RecipientName { get; set; } // اسم المستقبل

        public string SenderName { get; set; }
        public string? RejectionReason { get; set; }


    }
}
