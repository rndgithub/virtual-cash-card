using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Model
{
    public interface IAccountRepository
    {
        IEnumerable<Account> GetAll();
        Account Find(int id);
        void Update(Account account);

    }
    public class AccountRepository: IAccountRepository
    {
        private readonly IAccountContext _accountContext;
        public AccountRepository(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public IEnumerable<Account> GetAll()
        {
            return _accountContext.Accounts;
        }

        public Account Find(int id)
        {
            return _accountContext.Accounts.FirstOrDefault(x=>x.Id == id);
        }

        public void Update(Account account)
        {
            //code goes here
        }

    }
}
