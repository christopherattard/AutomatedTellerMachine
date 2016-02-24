using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AutomatedTellerMachine.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public int CheckingAccountId { get; set; }
        public virtual CheckingAccount Account { get; set; }
    }

    public class TransferFundsViewModel
    {
        [Required]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        public int SourceAccountId { get; set; }

        [Required]
        [Display(Name = "Transfer to Account")]
        public int DestinationAccountId { get; set; }

        public IEnumerable<SelectListItem> DestinationAccountList
        {
            get
            {
                if (_destinationAccountList == null)
                {
                    ApplicationDbContext db = new ApplicationDbContext();
                    _destinationAccountList = (from data in db.CheckingAccounts
                                               where data.Id != this.SourceAccountId
                                               select data).ToList();
                }

                return new SelectList(_destinationAccountList, "Id", "AccountNumber", 0);
            }
        }

        private List<CheckingAccount> _destinationAccountList;
    }
}