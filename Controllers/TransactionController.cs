using CMSApp.Data;
using CMSApp.Models;
using CMSApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSApp.Controllers
{
    public class TransactionController : Controller
    {
        private readonly CMSDBContext dbContext;

        public TransactionController(CMSDBContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        [HttpGet]
        public async Task<IActionResult> CreateTansactions()
        {
            if (!User.Identity.IsAuthenticated)
            {
                
                return RedirectToAction("Login", "Account");
            }

            
            var users = await dbContext.Users
                .Where(u => !string.IsNullOrEmpty(u.JobTitle)) 
                .Select(u => new { u.Id, u.JobTitle, u.FullName }) 
                .ToListAsync();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            var viewModel = new CreateTansactionsViewModel
            {
                AvailableRecipients = users
                    .Where(u => u.Id != currentUserId) 
                    .Select(u => new SelectListItem
                    {
                        Value = u.Id,
                        Text = $"{u.JobTitle} - {u.FullName}" 
                    })
                    .ToList()
            };

            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> CreateTansactions(CreateTansactionsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                
                bool isTransactionNumberExists = await dbContext.Transactions
                    .AnyAsync(t => t.TransactionNumber == viewModel.TransactionNumber);

                if (isTransactionNumberExists)
                {
                    
                    ModelState.AddModelError("TransactionNumber", "رقم المعاملة موجود مسبقاً.");
                }
                else
                {
                   
                    var selectedRecipient = await dbContext.Users.FindAsync(viewModel.SelectedRecipientId); 

                    var transaction = new Transaction
                    {
                        TransactionNumber = viewModel.TransactionNumber,
                        TransactionType = viewModel.TransactionType,
                        TransactionStatus = viewModel.TransactionStatus,
                        TransactionNature = viewModel.TransactionNature,
                        Recipient = viewModel.SelectedRecipientId,
                        RecipientName = selectedRecipient.FullName,
                        RecipientJobTitle = selectedRecipient.JobTitle,
                        Notes = viewModel.Notes,
                        ReceptionDate = DateTime.Now,
                        UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
                    };

                    
                    if (viewModel.Attachments != null && viewModel.Attachments.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await viewModel.Attachments.CopyToAsync(memoryStream);
                            transaction.AttachmentData = memoryStream.ToArray();
                        }
                    }

                
                    await dbContext.Transactions.AddAsync(transaction);
                    await dbContext.SaveChangesAsync();

                   
                    return RedirectToAction("HomePage", "Account");
                }
            }

        
            return View(viewModel);
        }

        public async Task<IActionResult> TransactionList()
        {

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var transactions = await dbContext.Transactions
       .Where(t => t.ConductTransaction != "مرفوض"
                   && t.ConductTransaction != "مقبول"
                   && t.Recipient == currentUserId) 
       .Select(t => new ReceivingTransactions
       {
           TransactionNumber = t.TransactionNumber,
           TransactionStatus = t.TransactionStatus,
           Recipient = t.Recipient,
           ReceptionDate = t.ReceptionDate,
           IsAccepted = t.ConductTransaction == "مقبول",
           IsReturned = t.ConductTransaction == "تحت الإجراء",
           IsRejected = t.ConductTransaction == "مرفوض"
       })
                .ToListAsync();

            return View(transactions);
        }


        [HttpGet]
        public async Task<IActionResult> TransactionDetails(string transactionNumber)
        {
            var transaction = await dbContext.Transactions
                .Include(t => t.User) // Include the user details
                .FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);

            if (transaction == null)
            {
                return NotFound();
            }

            // Retrieve the sender's details using the correct property names
            var senderFullName = transaction.User?.FullName ?? "اسم المستخدم غير متاح"; // Fallback message
            var senderJobTitle = transaction.User?.JobTitle ?? "المسمى الوظيفي غير متاح"; // Fallback message

            var viewModel = new TransactionDetailsViewModel
            {
                FullName = senderFullName,
                JobTitle = senderJobTitle, // Use the correct job title property
                TransactionNumber = transaction.TransactionNumber,
                Recipient = transaction.Recipient,
                TransactionType = transaction.TransactionType,
                Notes = transaction.Notes,
                Attachments = transaction.AttachmentData // Adjust this if necessary
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> AcceptTransaction(string transactionNumber)
        {
            var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);
            if (transaction != null)
            {
                transaction.ConductTransaction = "مقبول";
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("TransactionList");
        }

        [HttpPost]
        public async Task<IActionResult> ReturnTransaction(string transactionNumber)
        {
            var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);
            if (transaction != null)
            {
                transaction.ConductTransaction = "تحت الإجراء";
                await dbContext.SaveChangesAsync();
            }
            return RedirectToAction("TransactionList");
        }

        [HttpPost]
        public async Task<IActionResult> RejectTransaction(string transactionNumber)
        {
            var transaction = await dbContext.Transactions.FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);
            if (transaction != null)
            {
                transaction.ConductTransaction = "مرفوض";
                await dbContext.SaveChangesAsync();

                return RedirectToAction("RejectionReason", new { transactionNumber = transaction.TransactionNumber });
            }
            return RedirectToAction("TransactionList");
        }

        [HttpGet]
        public IActionResult RejectionReason(string transactionNumber)
        {
            var viewModel = new RejectionReasonViewModel
            {
                TransactionNumber = transactionNumber 
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RejectionReason(RejectionReasonViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var transaction = await dbContext.Transactions
                                    .FirstOrDefaultAsync(t => t.TransactionNumber == viewModel.TransactionNumber);

                if (transaction != null)
                {
                    transaction.ConductTransaction = "مرفوض";
                    transaction.RejectionReason = viewModel.RejectionReason;

                    await dbContext.SaveChangesAsync();
                    return RedirectToAction("TransactionList");
                }
                else
                {
                    ModelState.AddModelError("", "لم يتم العثور على المعاملة.");
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadAttachment(string transactionNumber)
        {
            var transaction = await dbContext.Transactions
                .FirstOrDefaultAsync(t => t.TransactionNumber == transactionNumber);

            if (transaction == null || transaction.AttachmentData == null)
            {
                return NotFound();
            }

            string fileName = $"{transaction.TransactionNumber}_attachment";
            string contentType = "application/octet-stream";

            return File(transaction.AttachmentData, contentType, fileName);
        }

        public async Task<IActionResult> TrackTransactions()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            
            var transactions = await (from t in dbContext.Transactions
                                      join recipient in dbContext.Users
                                      on t.Recipient equals recipient.Id into recipientGroup
                                      from recipientUser in recipientGroup.DefaultIfEmpty()
                                      where t.UserId == currentUserId 
                                      select new TrackTransactionsViewModel
                                      {
                                          TransactionNumber = t.TransactionNumber,
                                          TransactionStatus = t.TransactionStatus,
                                          Recipient = t.Recipient,
                                          ReceptionDate = t.ReceptionDate,
                                          ConductTransaction = t.ConductTransaction,
                                          FullName = recipientUser != null ? recipientUser.FullName : "غير متاح",
                                          JobTitle = recipientUser != null ? recipientUser.JobTitle : "غير متاح",
                                          RejectionReason = t.RejectionReason

                                      }).ToListAsync();

            Console.WriteLine($"Number of transactions for current user: {transactions.Count}");

            return View(transactions);
        }


        [HttpGet]
        public IActionResult BackTrackTransactions()
        {
            return RedirectToAction("HomePage", "Account");
        }

        public async Task<IActionResult> AllTransaction()
        {
            var transactions = await (from t in dbContext.Transactions
                                      join sender in dbContext.Users on t.UserId equals sender.Id into senderGroup
                                      from senderUser in senderGroup.DefaultIfEmpty()

                                      join recipient in dbContext.Users on t.Recipient equals recipient.Id into recipientGroup
                                      from recipientUser in recipientGroup.DefaultIfEmpty()

                                      select new AllTransaction
                                      {
                                          TransactionNumber = t.TransactionNumber,
                                          TransactionType = t.TransactionType,
                                          TransactionStatus = t.TransactionStatus,
                                          TransactionNature = t.TransactionNature,
                                          UserId = t.UserId,
                                          ConductTransaction = t.ConductTransaction,
                                          ReceptionDate = t.ReceptionDate,

                                          
                                          SenderName = senderUser != null ? senderUser.FullName : "غير متاح",
                                          RecipientName = recipientUser != null ? recipientUser.FullName : "غير متاح",

                                           RejectionReason=t.RejectionReason
                                      }).ToListAsync();

            return View(transactions); 
        }

    }
}
