using System;

namespace CMSApp.ViewModels
{
    public class ReceivingTransactions
    {
        public string TransactionNumber { get; set; } 
        public string TransactionStatus { get; set; }   
        public string Recipient { get; set; }            
        public DateTime ReceptionDate { get; set; }      

        public bool IsAccepted { get; set; }
        public bool IsReturned { get; set; }
        public bool IsRejected { get; set; }

        public string RejectionReason { get; set; }


    }
}



