using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Model
{    public interface IAccountContext
    {
        HashSet<Account> Accounts { get; }
    }
    public class AccountContext:IAccountContext
    {
        private HashSet<Account> _accounts = new HashSet<Account>
        {
            new Account
            {
                Number = 123456,
                Pin = "1234",
                Balance = 0,
                AccountHolder = new Person { Name = "Joe Bloggs" }
            },
            new Account
            {
                Number = 456789,
                Pin = "4567",
                Balance = 0,
                AccountHolder = new Person { Name = "Fred Bloggs" }
            },
        };


        HashSet<Account> IAccountContext.Accounts => _accounts;

    }
}
