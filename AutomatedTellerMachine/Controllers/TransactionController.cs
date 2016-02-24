using AutomatedTellerMachine.Models;
using AutomatedTellerMachine.MVCServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomatedTellerMachine.Controllers
{
    [Authorize]
    public class TransactionController : Controller
    {
        private IApplicationDbContext db;

        public TransactionController()
        {
            db = new ApplicationDbContext();
        }

        public TransactionController(IApplicationDbContext dbContext)
        {
            db = dbContext;
        }

        // GET: Transaction/Deposit
        public ActionResult Deposit(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                var service = new CheckingAccountServices(db);
                service.UpdateBalance(transaction);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // GET: Transaction/Withdrawal
        public ActionResult Withdrawal(int checkingAccountId)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Withdrawal(Transaction transaction)
        {
            var checkingAccount = db.CheckingAccounts.Find(transaction.CheckingAccountId);
            if (checkingAccount.Balance < transaction.Amount)
            {
                ModelState.AddModelError("Amount", "Insufficient funds in source account");
            }

            if (ModelState.IsValid)
            {
                transaction.Amount = 0 - transaction.Amount;
                db.Transactions.Add(transaction);                
                db.SaveChanges();
                var service = new CheckingAccountServices(db);
                service.UpdateBalance(transaction);

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public ActionResult TransferFunds(int excludedAccountId)
        {
            TransferFundsViewModel tf = new TransferFundsViewModel();            
            tf.SourceAccountId = excludedAccountId;            
            return View(tf);
        }

        [HttpPost]
        public ActionResult TransferFunds(TransferFundsViewModel transferFunds)
        {
            CheckingAccount sourceAccount = db.CheckingAccounts.Find(transferFunds.SourceAccountId);
            if (sourceAccount.Balance < transferFunds.Amount)
            {
                ModelState.AddModelError("Amount", "Insufficient funds in source account");
                return View(transferFunds);
            }

            //Add source account transaction
            Transaction srcTransaction = new Transaction();
            srcTransaction.Account = sourceAccount;
            srcTransaction.CheckingAccountId = sourceAccount.Id;
            srcTransaction.Amount = 0 - transferFunds.Amount;
            db.Transactions.Add(srcTransaction);

            //Add destination account transaction
            Transaction destTransaction = new Transaction();
            CheckingAccount destAccount = db.CheckingAccounts.Find(transferFunds.DestinationAccountId);
            destTransaction.Account = destAccount;
            destTransaction.CheckingAccountId = destAccount.Id;
            destTransaction.Amount = transferFunds.Amount;
            db.Transactions.Add(destTransaction);

            //Save to db and update balances
            db.SaveChanges();
            var service = new CheckingAccountServices(db);
            service.UpdateBalance(srcTransaction);
            service.UpdateBalance(destTransaction);

            return RedirectToAction("Index", "Home");            
        }
    }
}