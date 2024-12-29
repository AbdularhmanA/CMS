namespace CMSApp.ViewModels
{
    public class TransactionDetailsViewModel
    {
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string TransactionNumber { get; set; }
        public string Recipient { get; set; }
        public string TransactionType { get; set; }
        public string Notes { get; set; }
        public byte[] Attachments { get; set; } // Adjust this type if necessary
    }
}