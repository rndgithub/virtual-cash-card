using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RNDITConsultants.Log
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(Exception ex);

    }
    public class Logger : ILogger
    {
        public void LogError(Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }

        public void LogInfo(string message)
        {
            Console.WriteLine(message);
        }
    }
}
