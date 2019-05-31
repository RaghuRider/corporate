using AIEnterprise.Foundation.DI;
using Sitecore;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace AIEnterprise.Foundation.Logging.Repositories
{
    [Service(typeof(ILogRepository))]
    public class LogRepository : ILogRepository
    {
        public void Debug(string message) => Log.Debug(message, this);

        public void Debug(string message, params object[] args) => Log.Debug(string.Format(message, args));

        public void Error(string message) => Log.Error(message, this);

        public void SingleError(string message) => Log.SingleError(message, this);

        public void SingleWarn(string message) => Log.SingleWarn(message, this);

        public void Info(string message) => Log.Info(message, this);

        public void Info(string message, params object[] args) => Log.Debug(string.Format(message, args));

        public void Warn(string message) => Log.Warn(message, this);

        public void Fatal(string message) => Log.Fatal(message, this);

        public void LogFormattedError(string stackTrace, [CallerMemberName] string MethodName = "", [CallerFilePath] string FilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {
            Log.Error("Error occured on the site[" + Context.Site.SiteInfo.Name.ToUpper() + "], in the method[" + MethodName + "],  in the file[" + FilePath + "], at line number[" + sourceLineNumber + "] and the Error is : " + stackTrace, this);

        }

    }
}