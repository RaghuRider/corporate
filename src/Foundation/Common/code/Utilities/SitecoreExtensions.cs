using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AIEnterprise.Foundation.Common.Utilities.SitecoreExtensions
{
    public static class SitecoreExtensions
    {
        public static bool IsDerived([NotNull] this TemplateItem template, [NotNull] ID templateId)
        {
            return template.ID == templateId || template.BaseTemplates.Any(baseTemplate => IsDerived(baseTemplate, templateId));
        }

        public static bool IsDerived([NotNull] this Item item, [NotNull] ID templateId)
        {
            return item.Template.BaseTemplates.Any(baseTemplate => IsDerived(baseTemplate, templateId));
        }

        public static string EnsurePostfixSlashForLink(this string link)
        {
            if (link.Contains("#") || link.Contains("?"))
            {
                return link;
            }
            return Sitecore.StringUtil.EnsurePostfix('/', link);
        }
    }
}