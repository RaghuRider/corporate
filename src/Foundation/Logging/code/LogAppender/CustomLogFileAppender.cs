using log4net.spi;
using Sitecore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIEnterprise.Foundation.Logging.LogAppender
{
    public class CustomLogFileAppender : log4net.Appender.SitecoreLogFileAppender
    {
        protected override void Append(LoggingEvent loggingEvent)
        {
            var properties = loggingEvent.Properties;
            properties["sitename"] = Context.Site.SiteInfo.Name.ToLower();
            //Environment.SetEnvironmentVariable("sitename", Context.Site.SiteInfo.Name.ToLower());
            base.Append(loggingEvent);
        }
    }
}