using log4net;
using AIEnterprise.Foundation.DI;
using AIEnterprise.Foundation.Logging.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIEnterprise.Foundation.Logging.Repositories
{
    [Service(typeof(ICustomLogger))]
    public class CustomLogger : ICustomLogger
    {
        public ILog _logger;

        public void LogMessage(string logAppender, string message, Logtype logtype)
        {
            try
            {
                _logger = LogManager.GetLogger(logAppender);
            }
            catch
            {
                _logger = LogManager.GetLogger(logAppender);
            }
            switch (logtype)
            {
                case Logtype.DEBUG:
                    _logger.Debug(message);
                    break;

                case Logtype.ERROR:
                    _logger.Error(message);
                    break;

                case Logtype.INFO:
                    _logger.Error(message);
                    break;

                case Logtype.WARN:
                    _logger.Error(message);
                    break;
                default:
                    break;

            }

        }
    }
}