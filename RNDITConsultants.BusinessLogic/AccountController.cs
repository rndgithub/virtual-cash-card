using RNDITConsultants.Log;
using RNDITConsultants.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.BusinessLogic
{
    public interface IAccountController
    {
        void TopUp(int accountNumber, decimal amount);
        void Withdraw(int accountNumber, string pin, decimal amount);
    }
    public class AccountController:IAccountController
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger _logger;
        private readonly object _gate = new object();

        public AccountController(IAccountRepository accountRepository, ILogger logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public void TopUp(int accountNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Only positive amount top up is allowed!");

            var account = _accountRepository.GetAll().FirstOrDefault(x => x.Number == accountNumber);
            if (account == null)
                throw new ArgumentException("Account cannot be found!");

            lock (_gate)
            {
                UpdateAccount(account, amount);
            }

            _logger.LogInfo("Account topped up.");
        }


        private void UpdateAccount(Account account, decimal amount)
        {
            account.Balance += amount;
            AddTransaction(amount, account);
            _accountRepository.Update(account);
        }

        public void Withdraw(int accountNumber, string pin, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Only positive amount withdrawal is allowed!");

            var account = _accountRepository.GetAll().FirstOrDefault(x => x.Number == accountNumber && x.Pin == pin);
            if (account == null)
                throw new ArgumentException("Account number/PIN combination is incorrect!");

            lock (_gate)
            {
                if (account.Balance < amount)
                    throw new ApplicationException("Not enough funds!");

                UpdateAccount(account, -amount);
            }

            _logger.LogInfo("Account updated.");

        }


        private void AddTransaction(decimal amount, Account account)
        {
            var transaction = new AccountTransaction { Account = account, Date = DateTime.Now, Amount = amount };
            account.Transactions.Add(transaction);
        }
    }
}
