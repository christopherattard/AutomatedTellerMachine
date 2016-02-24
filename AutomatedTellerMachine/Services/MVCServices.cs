using AutomatedTellerMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomatedTellerMachine.MVCServices
{
    public class CheckingAccountServices
    {
        private IApplicationDbContext db;

        public CheckingAccountServices(IApplicationDbContext dbContext)
        {
            this.db = dbContext;
        }

        public void CreateCheckingAccount(string firstName, string lastName, string userId, decimal initialBalance)
        {
            var accountNumber = (123456 + db.CheckingAccounts.Count()).ToString().PadLeft(10, '0');
            var checkingAccount = new CheckingAccount { FirstName = firstName, LastName = lastName, AccountNumber = accountNumber, Balance = initialBalance, ApplicationUserId = userId };
            db.CheckingAccounts.Add(checkingAccount);
            db.SaveChanges();            
        }

        public void UpdateBalance(Transaction transaction)
        {
            var checkingAccount = db.CheckingAccounts.Where(c=>c.Id == transaction.CheckingAccountId).First();
            checkingAccount.Balance += db.Transactions.Where(t => t.Id == transaction.Id).Sum(c => c.Amount);
            db.SaveChanges();
        }
    }    
}