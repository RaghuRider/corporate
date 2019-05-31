using AIEnterprise.Foundation.Logging.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIEnterprise.Foundation.Logging.Repositories
{
   public interface ICustomLogger
    {
        void LogMessage(string logAppender, string message, Logtype logtype);
    }
}
