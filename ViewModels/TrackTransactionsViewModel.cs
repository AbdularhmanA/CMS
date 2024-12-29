using System;

namespace CMSApp.ViewModels
{
    public class TrackTransactionsViewModel
    {
        public string TransactionNumber { get; set; }
        public string TransactionStatus { get; set; }
        public string Recipient { get; set; }
        public DateTime ReceptionDate { get; set; }
        public string ConductTransaction {  get; set; }

        public string? RejectionReason { get; set; }

        public string FullName { get; set; }
        public string JobTitle { get; set; }
    }
}
