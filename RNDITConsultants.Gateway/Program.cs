using Nancy.Hosting.Self;
using Ninject;
using RNDITConsultants.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Gateway
{
    class Program
    {
        static void Main(string[] args)
        {
            new StandardKernel(new CoreModule());

            using (var host = new NancyHost(new Uri("http://localhost:1234")))
            {
                host.Start(); //netsh http add urlacl url="http://+:1234/" user="Everyone"
                Console.WriteLine("Running on http://localhost:1234");
                Console.ReadLine();
            }
        }
    }
}
