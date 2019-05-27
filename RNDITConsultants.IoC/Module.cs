using Ninject.Modules;
using RNDITConsultants.BusinessLogic;
using RNDITConsultants.Log;
using RNDITConsultants.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.IoC
{
    public class CoreModule:NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ILogger>().To<Logger>();
            Kernel.Bind<IAccountContext>().To<AccountContext>();
            Kernel.Bind<IAccountRepository>().To<AccountRepository>();
            Kernel.Bind<IAccountController>().To<AccountController>();
        }
    }
}
