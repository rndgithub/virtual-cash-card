using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Model
{
    public class Account
    {
        public Account()
        {
            Transactions = new List<AccountTransaction>();
        }
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal Balance { get; set; }
        public string Pin { get; set; }
        public Person AccountHolder { get; set; }
        public ICollection<AccountTransaction> Transactions { get; set; }

    }
}
