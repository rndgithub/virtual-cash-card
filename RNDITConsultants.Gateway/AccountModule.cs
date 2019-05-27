using Nancy;
using Ninject;
using RNDITConsultants.BusinessLogic;
using RNDITConsultants.IoC;
using RNDITConsultants.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Gateway
{
    public class AccountModule: NancyModule
    {
        private readonly IAccountController _accountController;
        private readonly ILogger _logger;

        public AccountModule(IAccountController accountController, ILogger logger)
        {
            _accountController = accountController;
            _logger = logger;

            // http://localhost:1234/
            Get["/"] = (p) => $"Time is: {DateTime.Now}";

            // http://localhost:1234/Account/TopUp/123456/100
            Put["/Account/TopUp/{AccountNumber}/{Amount}"] = (p) => TopUp(p.AccountNumber, p.Amount);

            // http://localhost:1234/Account/Withdraw/123456/100/1234
            Put["/Account/Withdraw/{AccountNumber}/{Amount}/{Pin}"] = (p) => Withdraw(p.AccountNumber, p.Amount, p.Pin);
        }

        private dynamic TopUp(int accountNumber, decimal amount)
        {
            try
            {
                _accountController.TopUp(accountNumber, amount);
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return ex.Message;
            }
        }

        private dynamic Withdraw(int accountNumber, decimal amount, string pin)
        {
            try
            {
                _accountController.Withdraw(accountNumber, pin, amount);
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return ex.Message;
            }
        }

    }
}
